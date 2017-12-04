using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
namespace edtools.Remap.Transformation {
    public class SearchStringTransformation : BaseTransformation {
        Regex findRegex;
        bool first;
        int groupNumber;
        public SearchStringTransformation(string searchRegex, int groupNumber = 0, bool first = false) {
            this.findRegex = new Regex(searchRegex, RegexOptions.Compiled);
            this.first = first;
            this.groupNumber = groupNumber;
        }

        public override object TransformSingleObject(object inputObject) {
            string inputString = CheckType<string>(inputObject);
            if (first) {
                return findRegex.Match(inputString).Groups[groupNumber].Value;
            } else {
                IEnumerable<Match> results = findRegex.Matches(inputString).Cast<Match>();
                return results.Select(x =>
                    x.Groups[groupNumber].Value
                );
            }
        }
    }
}
