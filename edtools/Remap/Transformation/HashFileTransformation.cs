using System;
using System.IO;
using System.Security.Cryptography;

namespace edtools.Remap.Transformation {
    public class HashFileTransformation : BaseTransformation {

        HashAlgorithm algorithm;
        bool ignoreMissing;
        public HashFileTransformation(string algorithm, bool ignoreMissing) {
            this.algorithm = GetHashAlgorithm(algorithm);
            this.ignoreMissing = ignoreMissing;
        }

        public override string TransformValue(string inputValue) {
            try {
                using (var stream = File.OpenRead(inputValue)) {
                    byte[] hash = algorithm.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            } catch (FileNotFoundException) {
                if (!ignoreMissing) {
                    throw new FileNotFoundException(String.Format("Could not find file {0}", inputValue));
                }
                return "";
            }
        }
    }
}
