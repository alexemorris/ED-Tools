using System.Management.Automation;
using System.Collections.Generic;
using edtools.Utils;
using edtools.Remap;

namespace edtools.StructuredLoad {
    [Cmdlet(VerbsData.Edit, "ObjectMapping")]
    public class EditObjectMapping : Cmdlet {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public PSObject InputObject {
            get { return inputObject; }
            set { inputObject = value; }
        }
        private PSObject inputObject;

        [Parameter(Mandatory = true)]
        public FullMapping Mapping {
            get { return mapping; }
            set { mapping = value; }
        }
        private FullMapping mapping;

        protected override void ProcessRecord() {
            base.ProcessRecord();
            Dictionary<string, object> inputObject = TypeConversion.PSObjectToDict(InputObject);
            Dictionary<string, object> outputObject = Mapping.GetRow(inputObject);
            WriteObject(TypeConversion.DictToPSObject(outputObject));
        }

    }
}
