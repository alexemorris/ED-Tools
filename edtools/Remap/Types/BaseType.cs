using System.Collections.Generic;
using System.Linq;

namespace edtools.Remap.Types {
    public abstract class BaseType {
        public abstract object CastValue(string inputValue);
        public List<object> CastValues(List<string> inputRows) {
            return inputRows.Select(CastValue).ToList();
        }
    }
}
