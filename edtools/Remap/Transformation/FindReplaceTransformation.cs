using System.Text.RegularExpressions;

namespace edtools.Remap.Transformation {
    public class FindReplaceTransformation : BaseTransformation {
        Regex findRegex;
        string findString;
        string replaceString;
        public FindReplaceTransformation(string findString, string replaceString, bool regex) {
            this.findString = findString;
            this.replaceString = replaceString;
            if (regex) {
                this.findRegex = new Regex(findString, RegexOptions.Compiled);
            }
        }

        public override object TransformSingleObject(object inputObject) {
            string inputString = CheckType<string>(inputObject);
            if (findRegex != null) {
                return findRegex.Replace(inputString, this.replaceString);
            }
            return inputString.Replace(this.findString, this.replaceString);
        }

    }
}
