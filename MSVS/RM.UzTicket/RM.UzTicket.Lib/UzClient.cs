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
			return _httpHandler.CookieContainer.GetCookies(new Uri(_baseUrl))["_gv_sessid"].Value;
		}

		public async Task<Station[]> SearchStationsAsync(string name)
		{
			var path = $"purchase/station/{name}";
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
								["station_if_from"] = source.StationId.ToString(),
								["station_id_till"] = destination.StationId.ToString(),
								["date_dep"] = date.ToMmDdYyyyString(),
								["time_dep"] = "00:00",
								["time_dep_till"] = "",
								["another_ec"] = "0",
								["search"] = ""
							};

			var result = await GetJson("purchase/search/", data: data);

			if (result["error"].ReadAs<bool>())
			{
				throw new ResponseException(result["value"].ReadAs<string>());
			}

			return ModelBase.FromJsonArray<Train>(result["value"]);
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
			return new Dictionary<string, string>
			{
				["User-Agent"] = _userAgent,
				["GV-Ajax"] = "1",
				["GV-Referer"] = _baseUrl,
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
				var textContent = String.Join("\r\n", data.Select(kv => $"{kv.Key}={kv.Value}"));
				req.Content = new StringContent(textContent);
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
			return JsonValue.Parse(str);
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
	}
}
