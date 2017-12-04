using System;
using System.Security.Cryptography;
using System.Text;

namespace edtools.Remap.Transformation {
    public class HashStringTransformation : BaseTransformation {
        HashAlgorithm algorithm;
        public HashStringTransformation(string algorithm) {
            this.algorithm = GetHashAlgorithm(algorithm);
        }

        public override object TransformSingleObject(object inputObject) {
            string inputString = CheckType<string>(inputObject);
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
