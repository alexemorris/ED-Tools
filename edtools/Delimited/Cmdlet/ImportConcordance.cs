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
        private FileSystemCmdletProviderEncoding psEncoding;

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
        public char Quote {
            get { return quote; }
            set { quote = value; }
        }
        private char quote = '\u00FE';

        [Parameter()]
        public char Delimiter {
            get { return delimiter; }
            set { delimiter = value; }
        }
        private char delimiter = '\u0014';
        private StreamReader reader;
        private Stream stream;
        protected ParseDelimited parser;
        private string currentLine;

        private void checkEncoding(System.Text.Encoding encoding) {
            stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            reader = new StreamReader(stream, encoding);
            string firstLine = reader.ReadLine();
            parser.TestLine(firstLine);
            if (header == null) {
                parser.ReadHeader(firstLine);
                currentLine = reader.ReadLine();
            } else {
                currentLine = firstLine;
            }
        }

        protected override void BeginProcessing() {
            base.BeginProcessing();
            parser = new ParseDelimited(header: Header, quote: Quote, delimiter: Delimiter);
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
            while (currentLine != null) {
                try {
                    Dictionary<string, object> output = parser.ReadLine(currentLine);
                    if (Mapping != null) {
                        output = Mapping.GetRow(output);
                    }
                    WriteObject(TypeConversion.DictToPSObject(output));
                } catch (InvalidDataException err) {
                    WriteWarning(err.Message);
                }
                currentLine = reader.ReadLine();
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
