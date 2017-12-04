using System.Collections.Generic;

namespace edtools.Remap.Mapping {
    public class DirectMapping : BaseMapping {
        string fieldName;
        public DirectMapping(string fieldName) {
            this.fieldName = fieldName;
        }

        public override object GetValue(Dictionary<string, object> inputRow) {
            object output;
            if (!inputRow.TryGetValue(this.fieldName, out output)) {
                output = "";
            }
            return output;
        }

    }
}

