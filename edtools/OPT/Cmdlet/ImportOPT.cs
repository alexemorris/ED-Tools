using System.Management.Automation;
using System.IO;
using Microsoft.PowerShell.Commands;
using edtools.Utils;
using PsUtils;

namespace edtools.OPT {
    [Cmdlet(VerbsData.Import, "OPT")]
    public class ImportOPT : Cmdlet {
        [Parameter(Mandatory = true, Position = 0)]
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
        public char? Quote {
            get { return quote; }
            set { quote = value; }
        }
        private char? quote = null;

        [Parameter(Mandatory = false)]
        public char Delimiter {
            get { return delimiter; }
            set { delimiter = value; }
        }
        private char delimiter = ',';

        [Parameter(Mandatory = false)]
        public string Root {
            get { return this.root; }
            set { this.root = value; }
        }
        private string root = null;

        [Parameter(Mandatory = false)]
        public SwitchParameter NoFullPath {
            get { return this.noFullPath; }
            set { this.noFullPath = value; }
        }
        private bool noFullPath = false;

        private ParseOPT parser;
        private StreamReader reader;
        private Stream stream;
        protected override void BeginProcessing() {
            base.BeginProcessing();

            if (root == null && !noFullPath) {
                root = System.IO.Path.GetDirectoryName(Path);
            } else if (noFullPath) {
                root = null;
            }

            parser = new ParseOPT(quote: Quote, delimiter: Delimiter, root: root);

            stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            if (psEncoding == FileSystemCmdletProviderEncoding.Default) {
                reader = new StreamReader(stream, true);
            } else {
                reader = new StreamReader(stream, CmdletEncoding.Convert(psEncoding));
            }
        }

        protected override void ProcessRecord() {
            base.ProcessRecord();
            string line;
            while ((line = reader.ReadLine()) != null) {
                try {
                    OPTDocument output = parser.ReadLine(line);
                    if (output != null) {
                        WriteObject(output);
                    }
                } catch (InvalidDataException err) {
                    WriteWarning(err.ToString());
                }
            }
        }

        protected override void EndProcessing() {
            base.EndProcessing();

            OPTDocument output = parser.ReadEnd();
            if (output != null) {
                WriteObject(output);
            }

            reader.Close();
            stream.Close();
        }

        protected override void StopProcessing() {
            base.StopProcessing();
            EndProcessing();
        }
    }
}
