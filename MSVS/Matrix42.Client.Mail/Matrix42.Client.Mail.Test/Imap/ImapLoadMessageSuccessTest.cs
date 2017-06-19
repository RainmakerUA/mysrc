using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Matrix42.Client.Mail.Test.Imap
{
	[TestClass]
	[DeploymentItem("Files")]
	public class ImapLoadMessageSuccessTest : ImapLoadMessageTestBase
	{
		[TestMethod]
		public void Test000()
		{
			LoadAndAssertMessage("000.emr");
		}

		[TestMethod]
		public void Test001()
		{
			var parameters = MessageValidateParameters.GetDefault();
			parameters.BodyText = false;
			parameters.BodyHtml = false;
			LoadAndAssertMessage("001.emr", parameters);
		}
	}
}
