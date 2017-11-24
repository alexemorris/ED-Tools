using System.Management.Automation;

namespace edtools.OPT {
    [Cmdlet(VerbsData.ConvertFrom, "OPT")]
    class ConvertFromOPT : Cmdlet {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string InputString {
            get { return inputString; }
            set { inputString = value; }
        }
        private string inputString;
    
        [Parameter( Position = 1 )]
        public char? Quote {
            get { return quote; }
            set { quote = value; }
        }
        private char? quote = null;

        [Parameter(Position = 2)]
        public char Delimiter {
            get { return delimiter; }
            set { delimiter = value; }
        }
        private char delimiter = ',';

        private ParseOPT parser;
        protected override void BeginProcessing() {
            base.BeginProcessing();
            parser = new ParseOPT();
        }

        protected override void ProcessRecord() {
            base.ProcessRecord();
            OPTEntry output = parser.readLine(inputString);
            if (output != null) {
                WriteObject(output);
            }
        }

        protected override void EndProcessing() {
            base.EndProcessing();
            OPTEntry output = parser.readEnd();
            if (output != null) {
                WriteObject(output);
            }
        }
    }
}
