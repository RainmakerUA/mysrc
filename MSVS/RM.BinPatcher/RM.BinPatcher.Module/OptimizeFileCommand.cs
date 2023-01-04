using System.Management.Automation;
using RM.BinPatcher.Model;

namespace RM.BinPatcher.Module
{
	// TODO: More Cmdlets: Find-Pattern(+), Test-File

	[Cmdlet(VerbsCommon.Optimize, "File")]
	public class OptimizeFileCommand: Cmdlet
    {
		[Parameter(Position = 0, Mandatory = true)]
		[Alias("patch", "p")]
		public string PatchFile { get; set; }

		[Parameter(Position = 1, Mandatory = true)]
		[Alias("target", "file", "f")]
		public string TargetFile { get; set; }

		[Parameter(Position = 2, Mandatory = false)]
		[Alias("nb")]
		public bool NoBackup { get; set; }

	    protected override void BeginProcessing()
	    {
		    base.BeginProcessing();
			// TODO: Initialization

			WriteDebug($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Applying patch...");
		}

	    protected override void ProcessRecord()
	    {
		    base.ProcessRecord();
			// TODO: Process item in pipeline
			
			using (var stream = File.Open(TargetFile, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
			{
				using (var bakStream = File.OpenWrite(TargetFile + ".bak"))
				{
					stream.CopyTo(bakStream);
					stream.Position = 0;
				}

				var patch = Patch.Parse(File.ReadAllLines(PatchFile));

				using (var patcher = new Patcher(stream))
				{
					var result = patcher.Apply(patch);
					WriteInformation(result.IsSuccess ? "Patch is applied" : "Patch is NOT applied: " + result.Message, null);
				}
			}
		}

	    protected override void EndProcessing()
	    {
			// TODO: Finalization
			WriteDebug($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Application completed");

			base.EndProcessing();
	    }

	    protected override void StopProcessing()
	    {
			// TODO: Handle abnormal termination
		    base.StopProcessing();
	    }
    }
}
