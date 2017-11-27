using System.Management.Automation;
using System.Collections.Generic;
using System.IO;
using edtools.Utils;

namespace edtools.Delimited {
    [Cmdlet(VerbsData.Import, "Concordance")]
    public class ImportConcordance : Cmdlet {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string Path {
            get { return path; }
            set { path = value; }
        }
        private string path;

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
            string line;
            parser = new ParseDelimited(header: header);
            StreamReader file = new StreamReader(path);

            if (header == null && first) {
                parser.ReadHeader(file.ReadLine());
                first = false;
            } else {
                
            }
            while ((line = file.ReadLine()) != null) {
                try {
                    Dictionary<string, object> output = parser.ReadLine(line);
                    WriteObject(TypeConversion.DictToPSObject(output));
                } catch (InvalidDataException err) {
                    WriteWarning(err.ToString());
                }
            }
            file.Close();
        }
    }
}
