using System;
using System.IO;
using System.Management.Automation;

namespace RM.BinPatcher.Module
{
	[Cmdlet(VerbsCommon.Find, "Pattern")]
	public class FindPatternCommand: Cmdlet
	{
		[Parameter(Position = 0, Mandatory = true)]
		[Alias("pat", "p")]
		public string Pattern { get; set; }

		[Parameter(Position = 1, Mandatory = true)]
		[Alias("target", "file", "f")]
		public string TargetFile { get; set; }

		protected override void BeginProcessing()
		{
			base.BeginProcessing();

			WriteDebug($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Searching pattern matches...");
		}

		protected override void ProcessRecord()
		{
			var pattern = Model.Pattern.Parse(Pattern);

			using (var stream = File.Open(TargetFile, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (var patcher = new Patcher(stream))
				{
					WriteObject(patcher.FindPattern(pattern));
				}
			}
		}

		protected override void EndProcessing()
		{
			WriteDebug($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Search completed");

			base.EndProcessing();
		}
	}
}
