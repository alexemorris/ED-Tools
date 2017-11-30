using System.Management.Automation;
using System.Collections.Generic;
using edtools.Utils;

namespace edtools.Delimited {
    [Cmdlet(VerbsData.ConvertTo, "Concordance")]
    public class ConvertToConcordance : Cmdlet {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public PSObject InputObject {
            get { return inputObject; }
            set { inputObject = value; }
        }
        private PSObject inputObject;

        [Parameter(Mandatory = false)]
        public SwitchParameter NoHeader {
            get { return this.noHeader; }
            set { this.noHeader = value; }
        }
        private bool noHeader = false;

        [Parameter()]
        public char? Quote {
            get { return quote; }
            set { quote = value; }
        }
        private char? quote = '\u00FE';

        [Parameter()]
        public string[] Header {
            get { return header; }
            set { header = value; }
        }
        private string[] header = null;

        [Parameter()]
        public char Delimiter {
            get { return delimiter; }
            set { delimiter = value; }
        }
        private char delimiter = '\u0014';
        private bool first = true;

        private StringifyDelimited stringifier;
        protected override void BeginProcessing() {
            base.BeginProcessing();
            stringifier = new StringifyDelimited(header: Header, quote: Quote, delimiter: Delimiter);
        }

        protected override void ProcessRecord() {
            base.ProcessRecord();
            Dictionary<string, object> toWrite = TypeConversion.PSObjectToDict(InputObject);
            string line = stringifier.ReadLine(toWrite);
            if (!NoHeader && first) {
                WriteObject(stringifier.GetHeader());
                first = false;
            }
            WriteObject(line);
        }
    }
}
