
namespace RM.UzTicket.Lib.Exceptions
{
	public class HttpError : UzException
	{
		//public HttpError()
		//{
		//}

		//public HttpError(string message) : base(message)
		//{
		//}

		//public HttpError(string message, Exception innerException) : base(message, innerException)
		//{
		//}

		public HttpError(int statusCode, string responseBody, string requestData = null, string json = null)
			: base($"Status code: {statusCode}; request data: {requestData}; response body: {responseBody}")
		{
			StatusCode = statusCode;
			ResponseBody = responseBody;
			RequestData = requestData;
			Json = json;
		}

		public int StatusCode { get; }

		public string ResponseBody { get; }

		public string RequestData { get; }

		public string Json { get; }
	}
}
