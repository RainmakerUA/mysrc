namespace RM.UzTicket.Lib.Exceptions
{
	public class BadRequest : HttpError
	{
		public BadRequest(int statusCode, string responseBody, string requestData = null, string json = null)
			: base(statusCode, responseBody, requestData, json)
		{
		}
	}
}