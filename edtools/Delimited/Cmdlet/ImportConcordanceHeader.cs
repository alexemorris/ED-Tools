using System.Management.Automation;
using Microsoft.PowerShell.Commands;
using System.Collections.Generic;
using System.IO;
using edtools.Utils;
using edtools.Remap;
using PsUtils;
using System.Text;
using System;

namespace edtools.Delimited {
    [Cmdlet(VerbsData.Import, "ConcordanceHeader")]
    public class ImportConcordanceHeader : Cmdlet {
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
        private FileSystemCmdletProviderEncoding psEncoding;

        private StreamReader reader;
        private Stream stream;
        private ParseDelimited parser;
        private string[] header;

        private void checkEncoding(System.Text.Encoding encoding) {
            stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            reader = new StreamReader(stream, encoding);
            string firstLine = reader.ReadLine();
            parser.TestLine(firstLine);
            header = parser.ReadHeader(firstLine);
        }

        protected override void BeginProcessing() {
            base.BeginProcessing();
            parser = new ParseDelimited();
            WriteVerbose(String.Format("Determining encoding for {0}.", path));
            Encoding[] encodings = new System.Text.Encoding[] { CmdletEncoding.Convert(psEncoding), System.Text.Encoding.UTF8, System.Text.Encoding.ASCII, System.Text.Encoding.Default, System.Text.Encoding.UTF32 };

            int tried = 0;
            foreach (Encoding encoding in encodings) {
                try {
                    tried++;
                    WriteVerbose(String.Format("Trying {0}", encoding.EncodingName));
                    checkEncoding(encoding);
                    break;
                } catch (InvalidDataException e) {
                    stream.Close();
                    reader.Close();
                    if (psEncoding != FileSystemCmdletProviderEncoding.Unknown) {
                        throw new InvalidDataException(String.Format("Could not parse using {0} - ({1}). Ommiting encoding flag will try all common encodings.", encoding.EncodingName, e.Message));
                    } else if (tried == encodings.Length) {
                        throw new InvalidDataException(String.Format("Could not determine encoding ({0}). Check input data or specify a non standard encoding using the -Encoding flag.", e.Message));
                    }
                }
            }
            WriteVerbose(String.Format("Reading {0} with {1} encoding", path, reader.CurrentEncoding.EncodingName));
        }

        protected override void ProcessRecord() {
            base.ProcessRecord();
            WriteObject(header);          
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
