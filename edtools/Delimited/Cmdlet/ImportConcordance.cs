using System.Management.Automation;
using Microsoft.PowerShell.Commands;
using System.Collections.Generic;
using System.IO;
using edtools.Utils;
using edtools.Remap;
using PsUtils;

namespace edtools.Delimited {
    [Cmdlet(VerbsData.Import, "Concordance")]
    public class ImportConcordance : Cmdlet {
        [Parameter(Mandatory = true, Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Path {
            get { return path; }
            set { path = value; }
        }
        private string path;


        [Parameter(Mandatory = false)]
        public FileSystemCmdletProviderEncoding Encoding {
            get { return this.psEncoding; }
            set { this.psEncoding = value; }
        }
        private FileSystemCmdletProviderEncoding psEncoding = FileSystemCmdletProviderEncoding.Default;

        [Parameter(Mandatory = false)]
        public string[] Header {
            get { return header; }
            set { header = value; }
        }
        private string[] header = null;

        [Parameter(Mandatory = false)]
        public FullMapping Mapping {
            get { return mapping; }
            set { mapping = value; }
        }
        private FullMapping mapping = null;

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
        private StreamReader reader;
        private Stream stream;
        private ParseDelimited parser;
        protected override void BeginProcessing() {
            base.BeginProcessing();
            parser = new ParseDelimited(header: Header, quote: Quote, delimiter: Delimiter);
            stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            if (psEncoding == FileSystemCmdletProviderEncoding.Default) {
                reader = new StreamReader(stream, true);
            } else {
                reader = new StreamReader(stream, CmdletEncoding.Convert(psEncoding));
            }
        }

        protected override void ProcessRecord() {
            base.ProcessRecord();
            if (header == null && first) {
                parser.ReadHeader(reader.ReadLine());
                first = false;
            }
            string line;
            while ((line = reader.ReadLine()) != null) {
                try {
                    Dictionary<string, object> output = parser.ReadLine(line);
                    if (Mapping != null) {
                        output = Mapping.GetRow(output);
                    }
                    WriteObject(TypeConversion.DictToPSObject(output));
                } catch (InvalidDataException err) {
                    WriteWarning(err.Message);
                }
            }
        }

        protected override void EndProcessing() {
            base.EndProcessing();
            reader.Close();
            stream.Close();
        }

        protected override void StopProcessing() {
            base.StopProcessing();
            EndProcessing();
        }
    }
}
