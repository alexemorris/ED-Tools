using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace edtools.OPT
{
    public class ParseOPT {
        bool force;
        char delimiter;
        char? quote;
        List<string[]> currentBlock;
        public ParseOPT(char? quote = null, char delimiter = ',', bool force = false) {
            this.force = force;
            this.quote = quote;
            this.delimiter = delimiter;
            this.currentBlock = new List<string[]>();
        }

        private OPTEntry parseBlock() {
            if (currentBlock.Count() == 0) return null;
            string[] ids = currentBlock.Select(line => line[0]).ToArray();
            string[] paths = currentBlock.Select(line => line[2]).ToArray();
            if (!force) {
                try {
                    int count = Int32.Parse(currentBlock[0][6]);
                    if (count != currentBlock.Count) {
                        throw new InvalidDataException(String.Format("Page count for block {0} doesn't match what is specified.", currentBlock[0][0]));
                    }
                } catch (FormatException) {
                    throw new InvalidDataException(String.Format("No page count provided for block {0}", currentBlock[0][0]));
                }
            }
            currentBlock = new List<string[]>();
            return new OPTEntry(ids, paths);
        }

        private string[] splitLine(string line) {
            string[] currentLine = line.Split(this.delimiter).ToArray();
            if (this.quote.HasValue) {
                currentLine = currentLine.Select(col => col.TrimStart(this.quote.Value).TrimEnd(this.quote.Value)).ToArray();
            }
            if (currentLine.Length != 7) {
                throw new InvalidDataException(String.Format("Column count for block {0} is not 7", currentLine[0]));
            }
            return currentLine;
        }

        public OPTEntry readLine(string line) {
            string[] parsed = splitLine(line);
            OPTEntry output = null;
            if (parsed[3] == "Y" && currentBlock.Count != 0) {
                output = parseBlock();
            } 
            currentBlock.Add(parsed);
            return output;
        }

        public OPTEntry readEnd() {
            return parseBlock();
        }
    }
}
