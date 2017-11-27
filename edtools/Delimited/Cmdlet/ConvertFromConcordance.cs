using System.Management.Automation;
using System.Collections.Generic;
using System;
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
        public char? Quote {
            get { return quote; }
            set { quote = value; }
        }
        private char? quote = null;

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
        private char delimiter = ',';
        private bool first = true;

        private ParseDelimited parser;
        protected override void BeginProcessing() {
            base.BeginProcessing();
            parser = new ParseDelimited(header: header);
        }

        protected override void ProcessRecord() {
            base.ProcessRecord();
            if (header == null && first) {
                parser.ReadHeader(inputString);
                first = false;
            } else {
                try {
                    Dictionary<string, object> output = parser.ReadLine(inputString);
                    WriteObject(TypeConversion.DictToPSObject(output));
                } catch (InvalidDataException err) {
                    WriteWarning(err.ToString());
                }
            }
        }

        protected override void EndProcessing() {
            base.EndProcessing();
        }
    }
}
