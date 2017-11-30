using System.Management.Automation;
using Microsoft.PowerShell.Commands;
using System.IO;
using edtools.Utils;
using PsUtils;

namespace edtools.OPT {
    [Cmdlet(VerbsData.Export, "OPT")]
    public class ExportOPT : Cmdlet {
        [Parameter(ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public OPTDocument InputDocument {
            get { return inputObject; }
            set { inputObject = value; }
        }
        private OPTDocument inputObject;

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


        private Stream stream;
        private StreamWriter writer;
        protected override void BeginProcessing() {
            base.BeginProcessing();
            stream = File.Open(Path, Append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Write);
            writer = new StreamWriter(stream, CmdletEncoding.Convert(this.psEncoding));
        }

        protected override void ProcessRecord() {
            base.ProcessRecord();
            foreach (string line in inputObject.Stringify()) {
                writer.WriteLine(line);
            }
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
