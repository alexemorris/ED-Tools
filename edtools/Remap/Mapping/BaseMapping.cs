using System.Collections.Generic;
using System.Linq;


namespace edtools.Remap.Mapping {
    public abstract class BaseMapping {
        public abstract string GetValue(Dictionary<string, object> inputRow);
        public List<string> GetValues(List<Dictionary<string, object>> inputRows) {
            return inputRows.Select(GetValue).ToList();
        }

    }
}
