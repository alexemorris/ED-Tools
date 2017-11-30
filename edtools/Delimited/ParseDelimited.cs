using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace edtools.Delimited
{
    public class ParseDelimited {
        bool force;
        char delimiter;
        char? quote;
        string[] header;
        int lineNumber;
        public ParseDelimited(string[] header = null, char? quote = '\u00FE', char delimiter = '\u0014', bool force = false) {
            this.force = force;
            this.quote = quote;
            this.delimiter = delimiter;
            if (header != null) {
                parseHeader(header);
            }
            this.lineNumber = 0;
        }

        public string[] SplitLine(string line) {

            string[] currentLine = line.Split(this.delimiter).ToArray();
            if (this.quote.HasValue) {
                currentLine = currentLine.Select(col => col.TrimStart(this.quote.Value).TrimEnd(this.quote.Value)).ToArray();
            }
            return currentLine;
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

        public string[] ReadHeader(string line) {
            lineNumber++;
            string[] header = SplitLine(line);
            parseHeader(header);
            return this.header;
        }

        public string[] GetHeader() {
            return this.header;
        }

        public Dictionary<string, object> ReadLine(string line) {
            if (this.header == null) {
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

            int quotes = line.Length - line.Replace(quote.ToString(), String.Empty).Length;

            if (quotes != header.Length * 2) {
                throw new InvalidDataException(String.Format("Entry at line {0} has additional quote chars ({1}) (found {2}, expected {3}). Ignoring line.", lineNumber, quote, quotes, header.Length * 2));
            }

            for (int i = 0; i < this.header.Length; i++) {
                output.Add(this.header[i], inputLine[i]);
            }
            lineNumber++;
            return output;
        }
    }
}
