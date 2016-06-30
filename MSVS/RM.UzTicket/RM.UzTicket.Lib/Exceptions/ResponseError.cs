namespace RM.UzTicket.Lib.Exceptions
{
	public class ResponseError : HttpError
	{
		public ResponseError(int statusCode, string responseBody, string requestData = null, string json = null)
			: base(statusCode, responseBody, requestData, json)
		{
		}
	}
}