using System.Management.Automation;
using Microsoft.PowerShell.Commands;
using System.Collections.Generic;
using System.IO;
using edtools.Utils;
using PsUtils;

namespace edtools.Delimited {
    [Cmdlet(VerbsData.Export, "Concordance")]
    public class ExportConcordance : Cmdlet {
        [Parameter(ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public PSObject InputObject {
            get { return inputObject; }
            set { inputObject = value; }
        }
        private PSObject inputObject;

        [Parameter(Mandatory = true, Position = 0)]
        public string Path {
            get { return this.outputPath; }
            set { this.outputPath = value; }
        }
        private string outputPath;

        [Parameter(Mandatory = false)]
        public FileSystemCmdletProviderEncoding Encoding {
            get { return this.psEncoding; }
            set { this.psEncoding = value; }
        }
        private FileSystemCmdletProviderEncoding psEncoding = FileSystemCmdletProviderEncoding.Default;

        [Parameter(Mandatory = false)]
        public SwitchParameter Append {
            get { return this.append; }
            set { this.append = value; }
        }
        private bool append = false;

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
        private Stream stream;
        private StreamWriter writer;
        private StringifyDelimited stringifier;
        protected override void BeginProcessing() {
            base.BeginProcessing();
            stringifier = new StringifyDelimited(header: Header, quote: Quote, delimiter: Delimiter);
            stream = File.Open(Path, Append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Write);
            writer = new StreamWriter(stream, CmdletEncoding.Convert(this.psEncoding));
        }

        protected override void ProcessRecord() {
            base.ProcessRecord();
            Dictionary<string, object> toWrite = TypeConversion.PSObjectToDict(inputObject);
            string line = stringifier.ReadLine(toWrite);
            if (first && !Append) {
                writer.WriteLine(stringifier.GetHeader());
                first = false;
            }
            writer.WriteLine(line);
        }

        protected override void StopProcessing() {
            base.StopProcessing();
            writer.Close();
            stream.Close();
        }
        protected override void EndProcessing() {
            base.EndProcessing();
            writer.Close();
            stream.Close();
        }
    }
}
