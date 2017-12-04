using System.Linq;

namespace edtools.Remap.Transformation {
    public class SplitStringTransformation : BaseTransformation {
        string[] separator;
        bool ignoreMissing;
        public SplitStringTransformation(string separator, bool ignoreMissing = false) {
            this.separator = new string[] { separator };
            this.ignoreMissing = ignoreMissing;
        }
        public override object TransformSingleObject(object inputObject) {
            string input = CheckType<string>(inputObject);
            return input.Split(separator, ignoreMissing ? System.StringSplitOptions.RemoveEmptyEntries : System.StringSplitOptions.None)
                .Select(x => x.Trim())
                .Where(x => !ignoreMissing || (x.Length != 0))
                .ToList();
        }
    }
}
