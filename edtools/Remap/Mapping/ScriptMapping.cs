using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public override string GetValue(Dictionary<string, object> inputRow) {
            runspace.SessionStateProxy.SetVariable("inputValues", inputRow);
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(script);
            Collection<PSObject> PSOutput = pipeline.Invoke();
            return PSOutput.ToList()[0].ToString();
        }
    }
}
