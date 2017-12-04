using System.Collections.Generic;
using System.Linq;
using edtools.Remap.Mapping;
using edtools.Remap.Transformation;

namespace edtools.Remap {

    public class SingleMapping {
        BaseMapping mapping;
        BaseTransformation[] transformations;

        public SingleMapping(BaseMapping mapping, BaseTransformation[] transformations) {
            this.mapping = mapping;
            this.transformations = transformations;
        }

        public object GetValue(Dictionary<string, object> inputRow) {
            object inital = mapping.GetValue(inputRow);
            foreach(BaseTransformation transformation in transformations) {
                inital = transformation.TransformInputObject(inital);
            }
            return inital;
        }

        public List<object> GetValues(List<Dictionary<string, object>> inputRows) {
            return inputRows.Select(GetValue).ToList();
        }

    }
}
