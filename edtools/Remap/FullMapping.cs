using System;
using System.Collections.Generic;
using System.Linq;

namespace edtools.Remap {

    public class FullMapping {
        Tuple<string, SingleMapping>[] mappings;

        public FullMapping(Tuple<string, SingleMapping>[] mappings) {
            this.mappings = mappings;
        }

        public Dictionary<string, object> GetRow(Dictionary<string, object> inputRow) {
            Dictionary<string, object> output = new Dictionary<string, object>();
            foreach(Tuple<string, SingleMapping> mapping in mappings) {
                output.Add(mapping.Item1, mapping.Item2.GetValue(inputRow));
            }
            return output;
        }
        public List<Dictionary<string, object>> GetValues(List<Dictionary<string, object>> inputRows) {
            return inputRows.Select(GetRow).ToList();
        }
    }
}
