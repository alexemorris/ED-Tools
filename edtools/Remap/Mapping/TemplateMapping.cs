using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

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
                output = m.Groups[0].Value;
            }
            if (output is string) {
                return (string)output;
            } else {
                throw new InvalidCastException(String.Format("Object of type {0} passed to Template when expected a string", output.GetType()));
            }
        }
        
        public override object GetValue(Dictionary<string, object> inputRow) {
            this.currentValue = inputRow;
            return regex.Replace(this.template, evaluator);
        }
    }
}

