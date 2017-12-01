using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace edtools.Remap.Mapping {
    public class TemplateMapping : BaseMapping {
        string template;
        Dictionary<string, object> currentValue;
        MatchEvaluator evaluator;
        Regex regex = new Regex(@"\$\[([^\]]+)\]", RegexOptions.Compiled);
        public TemplateMapping(string template) {
            this.template = template;
            this.evaluator = new MatchEvaluator(SubstituteValue);
        }

        private string SubstituteValue(Match m) {
            object output;
            if (!this.currentValue.TryGetValue(m.Groups[1].Value, out output)) {
                output = "";
            }
            return output.ToString();
        }

        public override string GetValue(Dictionary<string, object> inputRow) {
            this.currentValue = inputRow;
            return regex.Replace(this.template, evaluator);
        }
    }
}

