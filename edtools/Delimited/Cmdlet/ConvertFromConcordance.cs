using System.Management.Automation;
using System.Collections.Generic;
using System.IO;
using edtools.Utils;

namespace edtools.Delimited {
    [Cmdlet(VerbsData.ConvertFrom, "Concordance")]
    public class ConvertFromConcordance : Cmdlet {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string InputString {
            get { return inputString; }
            set { inputString = value; }
        }
        private string inputString;


        [Parameter()]
        public string[] Header {
            get { return header; }
            set { header = value; }
        }
        private string[] header = null;

        [Parameter()]
        public char? Quote {
            get { return quote; }
            set { quote = value; }
        }
        private char? quote = '\u00FE';

        [Parameter()]
        public char Delimiter {
            get { return delimiter; }
            set { delimiter = value; }
        }
        private char delimiter = '\u0014';
        private bool first = true;

        private ParseDelimited parser;
        protected override void BeginProcessing() {
            base.BeginProcessing();
            parser = new ParseDelimited(header: Header, quote: Quote, delimiter: Delimiter);
        }

        protected override void ProcessRecord() {
            base.ProcessRecord();
            if (header == null && first) {
                parser.ReadHeader(InputString);
                first = false;
            } else {
                try {
                    Dictionary<string, object> output = parser.ReadLine(InputString);
                    WriteObject(TypeConversion.DictToPSObject(output));
                } catch (InvalidDataException err) {
                    WriteWarning(err.Message);
                }
            }
        }

        protected override void EndProcessing() {
            base.EndProcessing();
        }
    }
}
