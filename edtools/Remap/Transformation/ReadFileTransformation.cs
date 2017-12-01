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

        public override string TransformValue(string inputValue) {
            try {
                return File.ReadAllText(inputValue, fileEncoding);
            } catch (FileNotFoundException) {
                if (!ignoreMissing) {
                    throw new FileNotFoundException(String.Format("Could not find file {0}", inputValue));
                }
                return "";
            }
        }
    }
}
