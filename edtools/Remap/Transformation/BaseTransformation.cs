using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace edtools.Remap.Transformation {
    public enum HashAlgorithms { MD5, SHA256 };
    public abstract class BaseTransformation {
        public abstract string TransformValue(string inputValue);
        public List<string> TransformValues(List<string> inputRows) {
            return inputRows.Select(TransformValue).ToList();
        }
        public static HashAlgorithm GetHashAlgorithm(string algorithm) {
            switch(algorithm.ToLower()) {
                case "sha256":
                    return new SHA256CryptoServiceProvider();
                case "md5":
                default:
                    return new MD5CryptoServiceProvider();
            };
        }
    }

}
