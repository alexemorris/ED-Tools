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
        int lineNumber;
        public StringifyDelimited(string[] header = null, char? quote = '\u00FE', char delimiter = '\u0014', bool force = false) {
            this.force = force;
            this.quote = quote;
            this.delimiter = delimiter;
            this.lineNumber = 0;
            this.header = header;
        }

        public string[] GetHeader() {
            return this.header;
        }

        public string ReadLine(Dictionary<string, object> inputDict) {
            if (this.header == null) {
                this.header = inputDict.Keys.ToArray();
            }

            List<string> values = new List<string>();
            foreach (string column in this.header) {
                object val = "";
                inputDict.TryGetValue(column, out val);
                values.Add(val.ToString());
            }

            if (quote.HasValue) {
                values = values.Select(x => quote.Value.ToString() + x + quote.Value.ToString()).ToList();
            }
            return String.Join(delimiter.ToString(), values.ToArray());
        }
    }
}
