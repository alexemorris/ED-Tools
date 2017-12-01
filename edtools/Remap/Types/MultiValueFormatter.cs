using System;

namespace edtools.Remap.Types {
    public class MultiValueType : BaseType {
        string[] delimiter;
        bool removeEmpty;
        public MultiValueType(string delimiter, bool removeEmpty = true) {
            this.delimiter = new string[] { delimiter };
            this.removeEmpty = removeEmpty;
        }
        public override object CastValue(string inputValue) {
            return inputValue.Split(this.delimiter, this.removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }
    }
}
