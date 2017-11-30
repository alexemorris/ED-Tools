using System;
using System.IO;
using System.Linq;

namespace edtools.OPT
{
    public class ParseOPT {
        bool force;
        char delimiter;
        char? quote;
        OPTDocument currentDocument;
        string root = null;
        public ParseOPT(char? quote = null, char delimiter = ',', bool force = false, string root = null) {
            this.force = force;
            this.quote = quote;
            this.delimiter = delimiter;
            this.currentDocument = new OPTDocument();
            this.root = root;
        }

        private OPTPage parseLine(string line) {
            string[] currentLine = line.Split(this.delimiter).ToArray();
            if (this.quote.HasValue) {
                currentLine = currentLine.Select(col => col.TrimStart(this.quote.Value).TrimEnd(this.quote.Value)).ToArray();
            }
            if (currentLine.Length != 7) {
                throw new InvalidDataException(String.Format("Column count for block {0} is not 7", currentLine[0]));
            }

            OPTPage page =  new OPTPage {
                ID = currentLine[0],
                Volume = currentLine[1],
                Path = currentLine[2],
                First = currentLine[3] == "Y"
            };

            if (this.root != null) {
                string fullPath = Path.Combine(root, currentLine[2].TrimStart('\\').TrimStart('/'));
                if (!File.Exists(fullPath)) {
                    throw new FileNotFoundException(String.Format("Could not find TIFF at {0}", fullPath));
                } else {
                    page.FullPath = fullPath;
                }
            }
            return page;
        }

        public OPTDocument ReadLine(string line) {
            OPTPage page = parseLine(line);
            OPTDocument output = null;
            if (page.First && this.currentDocument.PageCount != 0) {
                output = this.currentDocument;
                this.currentDocument = new OPTDocument();
                this.currentDocument.AddPage(page);
            } else {
                this.currentDocument.AddPage(page);
            }
            return output;
        }

        public OPTDocument ReadEnd() {
            return this.currentDocument;
        }
    }
}
