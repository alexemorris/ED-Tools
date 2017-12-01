using System;
using System.Security.Cryptography;
using System.Text;

namespace edtools.Remap.Transformation {
    public class HashStringTransformation : BaseTransformation {
        HashAlgorithm algorithm;
        public HashStringTransformation(string algorithm) {
            this.algorithm = GetHashAlgorithm(algorithm);
        }

        public override string TransformValue(string inputValue) {
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputValue));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
