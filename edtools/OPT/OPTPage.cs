using System.Linq;

namespace edtools.OPT {
    public class OPTPage {
        public string ID { get; set; }
        public string Path { get; set; }
        public string FullPath { get; set; }
        public string Volume { get; set; }
        public bool First { get; set; }

        public string Stringify(string quote = null, string delimiter = ",", int count = 0) {
            string[] output = new string[] { ID, Volume, Path, count == 0 ? "" : "Y",
                "","", count == 0 ? "" : count.ToString() };
            if (quote != null) {
                output = output.Select(col => quote + col + quote).ToArray();
            }
            return string.Join(delimiter, output);
        }
    }
}
