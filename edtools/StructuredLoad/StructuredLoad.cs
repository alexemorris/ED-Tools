using edtools.OPT;
using System.Linq;
using System.Collections.Generic;

namespace edtools.StructuredLoad {
    class StructuredLoad {
        public List<StructuredLoadNode> Entries { get; set; }

        public StructuredLoad(List<Dictionary<string, object>> inputData, OPTDocument[] opts = null) {

            Dictionary<string, OPTDocument> optHash = new Dictionary<string, OPTDocument>();
            Dictionary<string, StructuredLoadNode> nodeHash = new Dictionary<string, StructuredLoadNode>();
            Dictionary<string, List<StructuredLoadNode>> parentHash = new Dictionary<string, List<StructuredLoadNode>>();

            if (opts != null) {
                opts.ToList().ForEach(x => optHash[x.ID] = x);
            }
            
            foreach (Dictionary<string, object> entry in inputData) {
                string ID = (string)entry["ID"];
                optHash.TryGetValue(ID, out OPTDocument opt);
                StructuredLoadNode node;
                if ((bool)entry["Is Email"]) {
                    node = new StructuredLoadEmail(entry);
                } else {
                    node = new StructuredLoadFile(entry);
                }
                nodeHash.Add(ID, node);

                if (!parentHash.ContainsKey(ID)) { parentHash.Add(ID, new List<StructuredLoadNode>());  }

                string parent = (string)entry["Parent"];
                if (parent != null) {
                    if (!parentHash.ContainsKey(parent)) {
                        parentHash.Add(parent, new List<StructuredLoadNode>());
                    }
                    parentHash[parent].Add(node);
                }
            }

            foreach(Dictionary < string, object > entry in inputData) {
                string ID = (string)entry["ID"];
                string Parent = (string)entry["Parent"];
                StructuredLoadNode node = nodeHash[ID];
                StructuredLoadNode parent = nodeHash[Parent];
                List<StructuredLoadNode> children = parentHash[ID];
                node.Parent = parent;
                node.Children = children;
            }
        }
    }
}
