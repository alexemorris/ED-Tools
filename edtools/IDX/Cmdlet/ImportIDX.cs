using System.Management.Automation;
using System.IO;
using System.IO.Compression;
using Microsoft.PowerShell.Commands;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using edtools.Utils;
using System;
using System.Collections.Generic;
using PsUtils;

namespace edtools.IDX {
    [Cmdlet(VerbsData.Import, "IDX")]
    public class ImportIDX : Cmdlet {
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

        [Parameter(Mandatory = false)]
        public SwitchParameter Parallel {
            get { return this.parallel; }
            set { this.parallel = value; }
        }
        private bool parallel = false;


        private FileSystemCmdletProviderEncoding psEncoding = FileSystemCmdletProviderEncoding.Default;

        private ParseIDX parser;
        private StreamReader reader;
        private Stream stream;
        protected override void BeginProcessing() {
            base.BeginProcessing();

            parser = new ParseIDX();

            try {
                Stream inStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 128 * 1024, FileOptions.SequentialScan);
                if (System.IO.Path.GetExtension(Path) == ".gz") {
                    stream = new GZipStream(inStream, CompressionMode.Decompress);
                } else {
                    stream = inStream;
                }
            } catch (Exception e) {
                WriteError(new ErrorRecord(e, "Could not open stream", ErrorCategory.OpenError, Path));
                return;
            }
            
        }

        protected override void ProcessRecord() {
            base.ProcessRecord();

            if (!parallel) {
                string line;
                if (psEncoding == FileSystemCmdletProviderEncoding.Default) {
                    reader = new StreamReader(stream, true);
                } else {
                    reader = new StreamReader(stream, CmdletEncoding.Convert(psEncoding));
                }

                while ((line = reader.ReadLine()) != null) {
                    Dictionary<string, object> output = parser.ReadLine(line);
                    if (output != null) {
                        WriteObject(TypeConversion.DictToPSObject(output));
                    }
                }
            } else {
                ConcurrentQueue<PSObject> outputQueue = new ConcurrentQueue<PSObject>();
                Task processingTask = Task.Factory.StartNew(() =>
                    System.Threading.Tasks.Parallel.ForEach(ParseIDX.splitIDX(stream, CmdletEncoding.Convert(psEncoding)), (byte[] docBytes) => {
                        string doc = CmdletEncoding.Convert(psEncoding).GetString(docBytes);
                        Dictionary<string, object> parsed = ParseIDX.ParseBlock(doc);
                        outputQueue.Enqueue(TypeConversion.DictToPSObject(parsed));
                    })
                 );
                 while (!processingTask.IsCompleted || !outputQueue.IsEmpty) {
                    if (outputQueue.TryDequeue(out PSObject output)) {
                        WriteObject(output);
                    } else {
                        System.Threading.Thread.Yield();
                    }
                 }

            }
        }

        protected override void EndProcessing() {
            base.EndProcessing();

            if (!parallel) {
                Dictionary<string, object> output = parser.ReadEnd();
                if (output != null) {
                    WriteObject(TypeConversion.DictToPSObject(output));
                }
                reader.Close();
            }
            stream.Close();
        }

        protected override void StopProcessing() {
            base.StopProcessing();
            EndProcessing();
        }
    }
}
