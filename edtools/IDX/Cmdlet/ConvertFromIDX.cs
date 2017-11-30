using System.Management.Automation;
using System.Collections.Generic;
using edtools.Utils;
namespace edtools.IDX {
    [Cmdlet(VerbsData.ConvertFrom, "IDX")]
    public class ConvertFromIDX : Cmdlet {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        public string InputString {
            get { return inputString; }
            set { inputString = value; }
        }
        private string inputString;

        private ParseIDX parser;
        protected override void BeginProcessing() {
            base.BeginProcessing();
            parser = new ParseIDX();
        }

        protected override void ProcessRecord() {
            base.ProcessRecord();
            Dictionary<string, object> output = parser.ReadLine(inputString);
            if (output != null) {
                WriteObject(TypeConversion.DictToPSObject(output));
            }
        }

        protected override void EndProcessing() {
            base.EndProcessing();
            Dictionary<string, object> output = parser.ReadEnd();
            if (output != null) {
                WriteObject(TypeConversion.DictToPSObject(output));
            }
        }
    }
}
