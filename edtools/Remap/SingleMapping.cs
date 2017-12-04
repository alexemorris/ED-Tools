using System.Collections.Generic;
using edtools.Remap.Mapping;
using edtools.Remap.Transformation;
using System.Linq;

namespace edtools.Remap {

    public enum MergeType { INPUT, OUTPUT, FAVOUR_INPUT, FAVOUR_OUTPUT }
    public class SingleMapping {
        BaseMapping mapping;
        BaseTransformation[] transformations;
        MergeType mergeType;
        public SingleMapping(BaseMapping mapping, BaseTransformation[] transformations, MergeType mergeType = MergeType.FAVOUR_INPUT) {
            this.mapping = mapping;
            this.mergeType = mergeType;
            this.transformations = transformations;
        }

        public object GetValue(Dictionary<string, object> inputRow, Dictionary<string, object> outputRow = null) {

            Dictionary<string, object> row;
            if (outputRow != null) {
                switch (mergeType) {
                    case MergeType.INPUT:
                        row = inputRow;
                        break;
                    case MergeType.OUTPUT:
                        row = outputRow;
                        break;
                    case MergeType.FAVOUR_INPUT:
                        row = new Dictionary<string, object>();
                        outputRow.ToList().ForEach(x => row[x.Key] = x.Value);
                        inputRow.ToList().ForEach(x => row[x.Key] = x.Value);
                        break;
                    case MergeType.FAVOUR_OUTPUT:
                        row = new Dictionary<string, object>();
                        inputRow.ToList().ForEach(x => row[x.Key] = x.Value);
                        outputRow.ToList().ForEach(x => row[x.Key] = x.Value);
                        break;
                    default:
                        row = inputRow;
                        break;
                }
            } else {
                row = inputRow;
            }
            

            object inital = mapping.GetValue(row);
            foreach(BaseTransformation transformation in transformations) {
                inital = transformation.TransformInputObject(inital);
            }
            return inital;
        }

    }
}
