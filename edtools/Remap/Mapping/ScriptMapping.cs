using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;

namespace edtools.Remap.Mapping {
    public class ScriptMapping : BaseMapping {

        string script;
        Runspace runspace;
        public ScriptMapping(string script) {
            this.script = script;
            this.runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
        }

        public override object GetValue(Dictionary<string, object> inputRow) {
            runspace.SessionStateProxy.SetVariable("inputValues", inputRow);
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(script);
            List<PSObject> PSOutput = pipeline.Invoke().ToList();
            if (PSOutput.Count() == 1) {
                return PSOutput[0].BaseObject;
            } else {
                return PSOutput.Select(x => x.BaseObject);
            }
        }
    }
}
