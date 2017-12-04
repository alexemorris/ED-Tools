using System;
using System.IO;
using System.Text;

namespace edtools.Remap.Transformation {
    public class ReadFileTransformation : BaseTransformation {
        Encoding fileEncoding;
        bool ignoreMissing;

        public ReadFileTransformation(string fileEncoding, bool ignoreMissing) {
            this.fileEncoding = GetEncoding(fileEncoding);
            this.ignoreMissing = ignoreMissing;
        }
        public ReadFileTransformation(Encoding fileEncoding, bool ignoreMissing) {
            this.fileEncoding = fileEncoding;
            this.ignoreMissing = ignoreMissing;
        }

        public ReadFileTransformation(bool ignoreMissing) {
            this.fileEncoding = null;
            this.ignoreMissing = ignoreMissing;
        }

        public override object TransformSingleObject(object inputObject) {
            string inputString;
            try {
                inputString = CheckType<FileInfo>(inputObject).FullName;
            } catch (InvalidCastException) {
                inputString = CheckType<string>(inputObject);
            }

            try {
                if (fileEncoding != null) {
                    return File.ReadAllText(inputString, fileEncoding);
                } else {
                    return File.ReadAllText(inputString);
                }
            } catch (FileNotFoundException) {
                if (!ignoreMissing) {
                    throw new FileNotFoundException(String.Format("Could not find file {0}", inputString));
                }
                return "";
            }
        }
    }
}
