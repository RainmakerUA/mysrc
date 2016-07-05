using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using RM.UzTicket.Lib.Exceptions;
using RM.UzTicket.Lib.Model;
using RM.UzTicket.Lib.Utils;

namespace RM.UzTicket.Lib
{
	internal class UzClient
	{
		private const string _baseUrl = "http://booking.uz.gov.ua/en"; // strict without trailing '/'!
		private const string _sessionIdKey = "_gv_sessid";
		private const int _requestTimeout = 10;
		private const int _tokenMaxAge = 600;

		private readonly AutoResetEvent _tokenLock;
		private HttpClientHandler _httpHandler;
		private HttpClient _httpClient;

		private string _token;
		private int _tokenTime;
		private string _userAgent;


		public UzClient()
		{
			_tokenLock = new AutoResetEvent(true);
			InitializeHttpClient();
		}

		public string GetSessionId()
		{
			var value = _httpHandler.CookieContainer.GetCookies(new Uri(_baseUrl))[_sessionIdKey].Value;
			return _sessionIdKey + "=" + value;
		}

		public async Task<Station[]> SearchStationsAsync(string name)
		{
			var path = $"purchase/station/{name}/";
			var json = await GetJson(path);
			return ModelBase.FromJsonArray<Station>(json["value"]);
		}

		public async Task<Station> FetchFirstStationAsync(string name)
		{
			var stations = await SearchStationsAsync(name);
			return stations.FirstOrDefault();
		}

		public async Task<Train[]> ListTrainsAsync(DateTime date, Station source, Station destination)
		{
			var data = new Dictionary<string, string>
							{
								["station_id_from"] = source.Id.ToString(),
								["station_id_till"] = destination.Id.ToString(),
								["date_dep"] = date.ToMmDdYyyyString(),
								["time_dep"] = "00:00",
								["time_dep_till"] = "",
								["another_ec"] = "0",
								["search"] = ""
							};

			var result = await GetJson("purchase/search/", data: data);

			return ModelBase.FromJsonArray<Train>(result["value"]);
		}

		public async Task<Train> FetchTrainAsync(DateTime date, Station source, Station destination, string trainNumber)
		{
			var trains = await ListTrainsAsync(date, source, destination);
			return trains.FirstOrDefault(tr => tr.Number == trainNumber);
		}

		public async Task<Coach[]> ListCoachesAsync(Train train, CoachType coachType)
		{
			var data = new Dictionary<string, string>
							{
								["station_id_from"] = train.SourceStation.Id.ToString(),
								["station_id_till"] = train.DestinationStation.Id.ToString(),
								["train"] = train.Number,
								["model"] = train.Model.ToString(),
								["date_dep"] = train.DepartureTime.Timestamp.ToString(),
								["round_trip"] = "0",
								["another_ec"] = "0",
								["coach_type"] = coachType.Letter
							};
			var result = await GetJson("purchase/coaches/", data: data);
			return ModelBase.FromJsonArray<Coach>(result["coaches"]);
		}

		public async Task<int[]> ListSeatsAsync(Train train, Coach coach)
		{
			var data = new Dictionary<string, string>
							{
								["station_id_from"] = train.SourceStation.Id.ToString(),
								["station_id_till"] = train.DestinationStation.Id.ToString(),
								["train"] = train.Number,
								["coach_num"] = coach.Number.ToString(),
								["coach_class"] = coach.Class,
								["coach_type_id"] = coach.TypeId.ToString(),
								["date_dep"] = train.DepartureTime.Timestamp.ToString()
							};
			var result = await GetJson("purchase/coach/", data: data);
			return ConcatValuesAndParseInt32(result["value"]["places"] as JsonObject);
		}

		public async Task<JsonValue> BookSeatAsync(Train train, Coach coach, int seat, string firstName, string lastName, bool? bedding = null)
		{
			var addBedding = bedding ?? coach.HasBedding;
			var data = new Dictionary<string, string>
							{
								["code_station_from"] = train.SourceStation.Id.ToString(),
								["code_station_to"] = train.DestinationStation.Id.ToString(),
								["train"] = train.Number,
								["date"] = train.DepartureTime.Timestamp.ToString(),
								["round_trip"] = "0"
							};
			var place = new Dictionary<string, string>
							{
								["ord"] = "0",
								["coach_num"] = coach.Number.ToString(),
								["coach_class"] = coach.Class,
								["coach_type_id"] = coach.TypeId.ToString(),
								["place_num"] = seat.ToString(),
								["firstname"] = firstName,
								["lastname"] = lastName,
								["bedding"] = addBedding ? "1" : "0",
								["child"] = "",
								["stud"] = "",
								["transp"] = "0",
								["reserve"] = "0"
							};
			foreach (var key in place.Keys)
			{
				data[$"places[0][{key}]"] = place[key];
			}

			var result = await GetJson("cart/add/", data: data);
			return result;
		}

		private async Task<string> GetTokenAsync()
		{
			if (IsTokenOutdated())
			{
				using (new AsyncLock(_tokenLock))
				{
					if (IsTokenOutdated())
					{
						InitializeHttpClient();

						var resp = await GetString("", null, new Dictionary<string, string> { ["User-Agent"] = _userAgent }, null);
						_token = TokenParser.ParseGvToken(resp);

						if (String.IsNullOrEmpty(_token))
						{
							throw new TokenException(resp);
						}

						_tokenTime = DateTimeExtensions.GetUnixTime();
					}
				}
			}

			return _token;
		}

		private async Task<IDictionary<string, string>> GetDefaultHeadersAsync()
		{
			var referer = _baseUrl + "/";
			return new Dictionary<string, string>
						{
							["User-Agent"] = _userAgent,
							//["Referer"] = referer,
							["GV-Ajax"] = "1",
							["GV-Referer"] = referer,
							//["GV-Screen"] = "1366x768",
							["GV-Token"] = await GetTokenAsync()
						};
		}

		private async Task<string> GetString(string path, HttpMethod method, IDictionary<string, string> headers,
											IDictionary<string, string> data)
		{
			var url = GetUrl(path);
			var req = new HttpRequestMessage(method ?? HttpMethod.Post, url);

			if (data != null && data.Count > 0)
			{
				req.Content = new FormUrlEncodedContent(data);
			}

			if (headers == null)
			{
				headers = await GetDefaultHeadersAsync();
			}

			SetHeaders(req.Headers, headers);

			Logger.Debug($"Fetching: {url}");
			Logger.Debug($"Headers: {String.Join("\n", headers.Select(kv => $"{kv.Key}: {kv.Value}"))}");
			Logger.Debug($"Cookies: {String.Join("\n", _httpHandler.CookieContainer.GetCookies(new Uri(_baseUrl)).Cast<Cookie>().Select(c => c.ToString()))}");

			var resp = await _httpClient.SendAsync(req);

			if (!resp.IsSuccessStatusCode)
			{
				if (resp.StatusCode == HttpStatusCode.BadRequest)
				{
					throw new BadRequestException((int)resp.StatusCode, null);
				}

				throw new HttpException((int)resp.StatusCode, null);
			}

			return await resp.Content.ReadAsStringAsync();
		}

		private async Task<JsonValue> GetJson(string path, HttpMethod method = null, IDictionary<string, string> headers = null,
											IDictionary<string, string> data = null)
		{
			var str = await GetString(path, method, headers, data);
			var json = JsonValue.Parse(str);

			if (json.ContainsKey("error") && json["error"] != null && json["error"].ReadAs<bool>())
			{
				throw new ResponseException(json["value"].ReadAs<string>());
			}

			return json;
		}

		private void InitializeHttpClient()
		{
			_userAgent = UserAgentSelector.GetRandomAgent();
			_httpHandler = new HttpClientHandler();
			_httpClient = new HttpClient(_httpHandler)
			{
				BaseAddress = new Uri(_baseUrl),
				Timeout = TimeSpan.FromSeconds(_requestTimeout)
			};
		}

		private bool IsTokenOutdated()
		{
			var unixNow = DateTimeExtensions.GetUnixTime();
			return unixNow - _tokenTime > _tokenMaxAge;
		}

		private string GetUrl(string relPath)
		{
			return $"{_baseUrl}/{relPath}";
		}

		private static void SetHeaders(HttpRequestHeaders headers, IDictionary<string, string> dict)
		{
			foreach (var key in dict.Keys)
			{
				headers.Add(key, dict[key]);
			}
		}

		private static int[] ConcatValuesAndParseInt32(IDictionary<string, JsonValue> values)
		{
			if (values != null)
			{
				var strings = Enumerable.Empty<string>();

				foreach (var value in values.Values)
				{
					strings = strings.Concat((value as IEnumerable<JsonValue>).Select(jv => jv.ReadAs<string>()));
				}

				return strings.Select(Int32.Parse).ToArray();
			}

			return new int[0];
		}
	}
}
