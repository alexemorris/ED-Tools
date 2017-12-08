using System.Management.Automation;


namespace edtools.Delimited {
    [Cmdlet(VerbsData.Import, "ConcordanceHeader")]
    public class ImportConcordanceHeader : ImportConcordance  {
        protected override void ProcessRecord() {
            WriteObject(parser.GetHeader());
        }

    }
}
