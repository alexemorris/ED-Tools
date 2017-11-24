namespace edtools.OPT {
    public class OPTEntry {
        string[] paths;
        string[] ids;
        string id;
        public OPTEntry(string[] ids, string[] paths) {
            this.paths = paths;
            this.ids = ids;
            this.id = ids[0];
        }

        public int GetPageCount() {
            return this.paths.Length;
        }

        public string[] GetPaths() {
            return this.paths;
        }

        public string GetID() {
            return this.id;
        }
    }
}
