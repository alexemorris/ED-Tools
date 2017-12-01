using Microsoft.VisualStudio.TestTools.UnitTesting;
using edtools.Remap.Transformation;
using System.Security.Cryptography;

namespace edtools.test.StructuredLoad {
    [TestClass]
    public class StructuredLoadTransformationTests {
        [TestMethod]
        public void HashStringTransformationWorks() {
            HashStringTransformation transformation = new HashStringTransformation("Md5");
            string input = "test";
            string expected = "098f6bcd4621d373cade4e832627b4f6";
            object actual = transformation.TransformValue(input);
            Assert.AreEqual(expected, actual, "Hash string transformation didn't get the correct value");
        }       
    }
}
