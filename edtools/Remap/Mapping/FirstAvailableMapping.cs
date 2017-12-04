using System.Collections.Generic;

namespace edtools.Remap.Mapping {
    public class FirstAvailableMapping : BaseMapping {
        string[] fieldNames;
        public FirstAvailableMapping(string[] fieldNames) {
            this.fieldNames = fieldNames;
        }

        public override object GetValue(Dictionary<string, object> inputRow) {
            object output;
            foreach (string fieldName in fieldNames) {
                if (inputRow.TryGetValue(fieldName, out output)) {
                    string stringRep = output as string;
                    if (stringRep != null) {
                        string trimmed = stringRep.Trim();
                        if (trimmed != "") {
                            return trimmed;
                        } else {
                            return null;
                        }
                    }
                    return output;
                }
            }
            return null;
        }
    }
}

