using System;
using System.Collections.Generic;

namespace edtools.Remap {

    public class FullMapping {
        List<Tuple<string, SingleMapping>> mappings;

        public FullMapping() {
            this.mappings = new List<Tuple<string, SingleMapping>>();
        }

        public FullMapping(List<Tuple<string, SingleMapping>> mappings) {
            this.mappings = mappings;
        }

        public void AddMapping(string name, SingleMapping mapping) {
            this.mappings.Add(new Tuple<string, SingleMapping>(name, mapping));
        }

        public Dictionary<string, object> GetRow(Dictionary<string, object> inputRow) {
            Dictionary<string, object> output = new Dictionary<string, object>();
            foreach(Tuple<string, SingleMapping> mapping in mappings) {
                object newValue = mapping.Item2.GetValue(inputRow, output);
                output.Add(mapping.Item1, newValue);
            }
            return output;
        }

    }
}
