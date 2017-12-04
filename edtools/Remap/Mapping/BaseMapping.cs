using System.Collections.Generic;
using System.Linq;

namespace edtools.Remap.Mapping {
    public abstract class BaseMapping {
        public abstract object GetValue(Dictionary<string, object> inputRow);

    }
}
