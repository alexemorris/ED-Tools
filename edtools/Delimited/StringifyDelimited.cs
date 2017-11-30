using System;
using System.Collections.Generic;
using System.Linq;

namespace edtools.Delimited
{
    public class StringifyDelimited {
        bool force;
        char delimiter;
        char? quote;
        string[] header;
        int lineNumber = 0;
        public StringifyDelimited(string[] header = null, char? quote = '\u00FE', char delimiter = '\u0014', bool force = false) {
            this.force = force;
            this.quote = quote;
            this.delimiter = delimiter;
            this.header = header;
        }

        public int LineNumber { get => lineNumber; set => lineNumber = value; }

        public string GetHeader() {
            return ProcessLine(this.header);
        }

        public string ProcessLine(string[] input) {
            string[] values = input;
            if (quote.HasValue) {
                values = values.Select(x => quote.Value.ToString() + x + quote.Value.ToString()).ToArray();
            }
            return String.Join(delimiter.ToString(), values.ToArray());
        }

        public string ReadLine(Dictionary<string, object> inputDict) {
            if (this.header == null) {
                this.header = inputDict.Keys.ToArray();
            }

            List<string> values = new List<string>();
            foreach (string column in this.header) {
                object val;
                if (!inputDict.TryGetValue(column, out val)) {
                    val = "";
                }
                values.Add(val.ToString());
            }

            LineNumber++;
            return ProcessLine(values.ToArray());
        }
    }
}
