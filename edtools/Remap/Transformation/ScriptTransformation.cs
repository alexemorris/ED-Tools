using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
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

        public override string TransformValue(string inputValue) {
            runspace.SessionStateProxy.SetVariable("InputValue", inputValue);
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(script);
            Collection<PSObject> PSOutput = pipeline.Invoke();
            return PSOutput.ToList()[0].ToString();
        }
    }
}
