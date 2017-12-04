using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace edtools.Remap.Transformation {
    public class ScriptTransformation : BaseTransformation {

        string script;
        Runspace runspace;
        public ScriptTransformation(string script) {
            this.script = script;
            this.runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
        }

        public override object TransformSingleObject(object inputObject) {
            runspace.SessionStateProxy.SetVariable("inputValue", inputObject);
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
