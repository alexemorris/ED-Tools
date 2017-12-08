using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace edtools.Delimited
{
    public class ParseDelimited {
        bool force;
        char delimiter;
        char quote;
        string[] header;
        int lineNumber;
        public ParseDelimited(string[] header = null, char quote = '\u00FE', char delimiter = '\u0014', bool force = false) {
            this.force = force;
            this.quote = quote;
            this.delimiter = delimiter;
            if (header != null) {
                parseHeader(header);
            }
            lineNumber = 0;
        }

        public string[] SplitLine(string line) {
            string[] currentLine = line.Split(this.delimiter).ToArray();
            return currentLine.Select(col => col.TrimStart(this.quote).TrimEnd(this.quote)).ToArray();
        }


        private void parseHeader(string[] input) {
            var hd = new Dictionary<string, int>();
            for (int i = 0; i < input.Length; i++) {
                if (hd.ContainsKey(input[i])) {
                    input[i] = input[i] + '_' + hd[input[i]]++;
                }
                hd[input[i]] = 2;
            }
            header = input;
        }

        public void TestLine(string line) {
            if (CharCount(line, this.quote) == 0) {
                throw new InvalidDataException(String.Format("No quotes found in input data"));
            }
            if (CharCount(line, this.delimiter) == 0) {
                throw new InvalidDataException(String.Format("No separators found in input data"));
            }
        }

        public string[] ReadHeader(string line) {
            TestLine(line);
            lineNumber++;
            string[] header = SplitLine(line);
            parseHeader(header);
            return this.header;
        }

        public string[] GetHeader() {
            return header;
        }

        public int CharCount(string input, char check) {
            return input.ToCharArray().Where(c => c == this.quote).Count();
        }

        public Dictionary<string, object> ReadLine(string line) {
            if (header == null) {
                throw new ArgumentException("No header provided, cannot parse concordance data");
            }

            Dictionary<string, object> output = new Dictionary<string, object>();

            if (String.IsNullOrWhiteSpace(line)) {
                throw new InvalidDataException(String.Format("Entry at line {0} is empty", lineNumber));
            }

            string[] inputLine = SplitLine(line);

            if (inputLine.Length != header.Length) {
                throw new InvalidDataException(String.Format("Entry at line {0} has {1} columns (expected {2}). Ignoring line.", lineNumber, inputLine.Length, header.Length));
            }

            int quotes = CharCount(line, quote);

            if (quotes > header.Length * 2) {
                throw new InvalidDataException(String.Format("Entry at line {0} has additional quote chars ({1}) (found {2}, expected {3}). Ignoring line.", lineNumber, quote, quotes, header.Length * 2));
            }
            if (quotes < header.Length * 2) {
                throw new InvalidDataException(String.Format("Entry at line {0} has missing quote chars ({1}) (found {2}, expected {3}). Ignoring line.", lineNumber, quote, quotes, header.Length * 2));
            }

            for (int i = 0; i < this.header.Length; i++) {
                output.Add(this.header[i], inputLine[i]);
            }

            lineNumber++; return output;
        }
    }
}
