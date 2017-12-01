using System.Collections.Generic;

namespace edtools.Remap.Mapping {
    public class FirstAvailableMapping : BaseMapping {
        string[] fieldNames;
        public FirstAvailableMapping(string[] fieldNames) {
            this.fieldNames = fieldNames;
        }

        public override string GetValue(Dictionary<string, object> inputRow) {
            object output;
            foreach (string fieldName in fieldNames) {
                if (inputRow.TryGetValue(fieldName, out output)) {
                    string trimmed = output.ToString().Trim();
                    if (trimmed != "") {
                        return trimmed;
                    }
                }
            }
            return "";
        }
    }
}

