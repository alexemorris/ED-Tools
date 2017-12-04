using System;
using System.IO;
using System.Text;

namespace edtools.Remap.Transformation {
    public class ReadFileTransformation : BaseTransformation {
        Encoding fileEncoding;
        bool ignoreMissing;
        public ReadFileTransformation(Encoding fileEncoding, bool ignoreMissing) {
            this.fileEncoding = fileEncoding;
            this.ignoreMissing = ignoreMissing;
        }

        public override object TransformSingleObject(object inputObject) {
            string inputString = CheckType<string>(inputObject);
            try {
                return File.ReadAllText(inputString, fileEncoding);
            } catch (FileNotFoundException) {
                if (!ignoreMissing) {
                    throw new FileNotFoundException(String.Format("Could not find file {0}", inputString));
                }
                return "";
            }
        }
    }
}
