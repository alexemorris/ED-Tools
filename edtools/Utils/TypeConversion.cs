using System.Collections.Generic;
using System.Management.Automation;
using System.Linq;

namespace edtools.Utils {
    class TypeConversion {
        public static PSObject DictToPSObject(Dictionary<string, object> inputDict) {
            PSObject output = new PSObject();
            foreach (KeyValuePair<string, object> entry in inputDict) {
                output.Members.Add(new PSNoteProperty(entry.Key, entry.Value));
            }
            return output;
        }

        public static Dictionary<string, object> PSObjectToDict(PSObject inputObj) {
            return inputObj.Properties.Where(p => (p.MemberType == PSMemberTypes.NoteProperty || p.MemberType == PSMemberTypes.AliasProperty) && p.Value != null).ToDictionary(p => p.Name, p => p.Value);
        }



    }
}
