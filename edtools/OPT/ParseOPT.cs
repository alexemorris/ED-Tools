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
        public ParseOPT(char? quote = null, char delimiter = ',', bool force = false) {
            this.force = force;
            this.quote = quote;
            this.delimiter = delimiter;
            this.currentDocument = new OPTDocument();
        }

        private OPTPage parseLine(string line) {
            string[] currentLine = line.Split(this.delimiter).ToArray();
            if (this.quote.HasValue) {
                currentLine = currentLine.Select(col => col.TrimStart(this.quote.Value).TrimEnd(this.quote.Value)).ToArray();
            }
            if (currentLine.Length != 7) {
                throw new InvalidDataException(String.Format("Column count for block {0} is not 7", currentLine[0]));
            }

            return new OPTPage {
                ID = currentLine[0],
                Volume = currentLine[1],
                Path = currentLine[2],
                First = currentLine[3] == "Y"
            };
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
