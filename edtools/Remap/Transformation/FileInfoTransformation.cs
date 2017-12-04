using System.Linq;
using System.IO;

namespace edtools.Remap.Transformation {
    public class FileInfoTransformation : BaseTransformation {
        string root;
        bool checkExists;
        public FileInfoTransformation(bool checkExists = true, string root = null) {
            this.root = root;
            this.checkExists = checkExists;
        }

        private string getPath(string input) {
            if (root == null) {
                return input;
            } else {
                if (input.StartsWith("/")) {
                    input.TrimStart('/');
                }
                return Path.GetFullPath(Path.Combine(root, input));
            }
        }
        public override object TransformSingleObject(object inputObject) {
            string input = CheckType<string>(inputObject);
            FileInfo file = new FileInfo(getPath(input));

            if (checkExists && !file.Exists) {
                throw new FileNotFoundException(string.Format("Could not find file at {0}", file.FullName));
            }
            return file;
        }
    }
}
