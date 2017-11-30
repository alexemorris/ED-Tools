using System.Management.Automation;
namespace edtools.OPT {
    [Cmdlet(VerbsData.ConvertTo, "OPT")]
    public class ConvertToOPT : Cmdlet {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public OPTDocument InputObject {
            get { return inputObject; }
            set { inputObject = value; }
        }
        private OPTDocument inputObject;

        protected override void BeginProcessing() {
            base.BeginProcessing();
        }

        protected override void ProcessRecord() {
            base.ProcessRecord();
            WriteObject(InputObject.Stringify());
        }
    }
}
