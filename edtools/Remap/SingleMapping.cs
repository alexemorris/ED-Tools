using System;
using System.Collections.Generic;
using System.Linq;
using edtools.Remap.Types;
using edtools.Remap.Mapping;
using edtools.Remap.Transformation;

namespace edtools.Remap {

    public class SingleMapping {
        BaseMapping mapping;
        BaseType formatter;
        BaseTransformation[] transformations;

        public SingleMapping(BaseMapping mapping, BaseTransformation[] transformations, BaseType formatter) {
            this.mapping = mapping;
            this.transformations = transformations;
            this.formatter = formatter;
        }

        public object GetValue(Dictionary<string, object> inputRow) {
            string inital = mapping.GetValue(inputRow);
            foreach(BaseTransformation transformation in transformations) {
                inital = transformation.TransformValue(inital);
            }
            return formatter.CastValue(inital);
        }

        public List<object> GetValues(List<Dictionary<string, object>> inputRows) {
            return inputRows.Select(GetValue).ToList();
        }

    }
}
