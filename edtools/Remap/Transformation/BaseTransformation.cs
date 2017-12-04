using System.Collections.Generic;
using System.Linq;
using System;
using System.Security.Cryptography;
using System.Text;
namespace edtools.Remap.Transformation {
    public abstract class BaseTransformation {
        public virtual object TransformInputObject(object inputObject) {

            // Checking if is a list of values, in which case transform each one
            IEnumerable<object> inputList = inputObject as IEnumerable<object>;
            if (inputList != null) {
                IEnumerable<object> output = inputList.Select(x => {
                    if (inputObject == null) {
                        return null;
                    } else {
                        return TransformSingleObject(x);
                    }
                });
                if (output.Count() == 1) {
                    return output.ToArray()[0];
                } else {
                    return output;
                }
            } else {
                if (inputObject == null) {
                    return null;
                }
                return TransformSingleObject(inputObject);
            }
        }

        public T CheckType<T>(object inputObject) {
            if (inputObject is T) {
                return (T)inputObject;
            } else {
                try {
                    return (T)Convert.ChangeType(inputObject, typeof(T));
                } catch (FormatException) {
                    throw new InvalidCastException(String.Format("Object of type {0} passed to Template when expected an object that can be converted to a {1}", inputObject.GetType(), typeof(T)));
                }
            }
        }

        public virtual object TransformSingleObject(object inputObject) {
            return inputObject;
        }
        public List<object> TransformInputObjects(List<object> inputRows) {
            return inputRows.Select(TransformInputObject).ToList();
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

        public static Encoding GetEncoding(string encoding) {
            switch (encoding.ToLower()) {
                case "ascii":
                    return Encoding.ASCII;
                case "utf8":
                default:
                    return Encoding.UTF8;
            };
        }
    }

}
