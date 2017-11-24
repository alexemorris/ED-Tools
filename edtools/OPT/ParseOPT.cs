using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace edtools.OPT
{
    public class ParseOPT {
        StringReader reader;
        bool force;
        char delimiter;
        char? quote;
        public ParseOPT(StringReader reader, char? quote = null, char delimiter = ',', bool force = false) {
            this.reader = reader;
            this.force = force;
            this.quote = quote;
            this.delimiter = delimiter;
        }

        private OPTEntry parseBlock(List<string[]> inputBlock) {
            string[] ids = inputBlock.Select(line => line[0]).ToArray();
            string[] paths = inputBlock.Select(line => line[2]).ToArray();
            if (!force) {
                try {
                    int count = Int32.Parse(inputBlock[0][6]);
                    if (count != inputBlock.Count) {
                        throw new InvalidDataException(String.Format("Page count for block {0} doesn't match what is specified.", inputBlock[0][0]));
                    }
                } catch (FormatException) {
                    throw new InvalidDataException(String.Format("No page count provided for block {0}", inputBlock[0][0]));
                }
            }
            
            return new OPTEntry(ids, paths);
        }

        private string[] getLine() {
            if (this.reader.Peek() >= 0) {
                string[] currentLine = this.reader.ReadLine().Split(this.delimiter).ToArray();
                if (this.quote.HasValue) {
                    currentLine = currentLine.Select(col => col.TrimStart(this.quote.Value).TrimEnd(this.quote.Value)).ToArray();
                }
                if (currentLine.Length != 7) {
                    throw new InvalidDataException(String.Format("Column count for block {0} is not 7", currentLine[0]));
                }
                return currentLine;
            } else {
                return null;
            }
        }

        public IEnumerable<OPTEntry> Entries() {
            List<string[]> currentBlock = new List<string[]>();
            string[] currentLine = getLine();
            while (currentLine != null) {
                do {
                    currentBlock.Add(currentLine);
                    currentLine = getLine();
                    if (currentLine == null) {
                        break;
                    }
                } while (currentLine[3] != "Y");
                yield return parseBlock(currentBlock);
                currentBlock = new List<string[]>();
            }
            yield break;
        }
    }
}
