
namespace RM.BinPatcher.Model
{
	public sealed class PatchResult
	{
		private PatchResult(bool isSuccess, string? message)
		{
			IsSuccess = isSuccess;
			Message = message;
		}

		public bool IsSuccess { get; }

		public string? Message { get; }

		internal static PatchResult MakeSuccess(string? message = null)
		{
			return new PatchResult(true, message);
		}

		internal static PatchResult MakeFail(string? message)
		{
			return new PatchResult(false, message);
		}
	}
}
