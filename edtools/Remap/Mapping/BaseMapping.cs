using System.Collections.Generic;
using System.Linq;

namespace edtools.Remap.Mapping {
    public abstract class BaseMapping {
        public abstract object GetValue(Dictionary<string, object> inputRow);
        public List<object> GetValues(List<Dictionary<string, object>> inputRows) {
            return inputRows.Select(GetValue).ToList();
        }
    }
}
