using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using edtools.Remap.Transformation;
using System;
namespace edtools.test.Remap {
    [TestClass]
    public class StructuredLoadTransformationTests {
        [TestMethod]
        public void HashStringTransformationWorks() {
            HashStringTransformation transformation = new HashStringTransformation("Md5");
            string input = "test";
            string expected = "098f6bcd4621d373cade4e832627b4f6";
            object actual = (string)transformation.TransformInputObject(new string[] { input });
            Assert.AreEqual(expected, actual, "Hash string transformation didn't get the correct value");
        }

        [TestMethod]
        public void FindReplaceTransformationWorks() {
            BaseTransformation transformation = new FindReplaceTransformation(@"\d{2}(\d{1})", "$1", true);
            string input = "test112test";
            string expected = "test2test";
            object actual = transformation.TransformInputObject(new string[] { input });

            Assert.AreEqual(expected, actual, "Hash string transformation didn't get the correct value");
        }


        [TestMethod]
        public void ConvertToDateTransformationWorks() {
            BaseTransformation transformation = new ConvertToDateTransformation("dd/MM/yyyy HH:mm:ss", true);
            string input = "16/01/2017 14:11:56";
            DateTime expected = new DateTime(2017, 1, 16, 14, 11, 56);
            object actual = transformation.TransformInputObject(input);
            Assert.AreEqual(expected, actual, "Date transformation didn't get the correct date");
        }

        [ExpectedException(typeof(FormatException))]
        [TestMethod]
        public void ConvertToDateTransformationInvalidDateThrows() {
            BaseTransformation transformation = new ConvertToDateTransformation("dd/MM/yyyy HH:mm:ss", false);
            string input = "16/01/27 14:11:56";
            object actual = transformation.TransformInputObject(input);
        }

        [TestMethod]
        public void ConvertToDateTransformationInvalidDateIgnores() {
            BaseTransformation transformation = new ConvertToDateTransformation("dd/MM/yyyy HH:mm:ss", true);
            string input = "16/01/27 14:11:56";
            object actual = transformation.TransformInputObject(input);
            object expected = null;
            Assert.AreEqual(expected, actual, "Date transformation didn't return null with invalid date input");
        }

        [TestMethod]
        public void ConvertFromDateTransformationWorks() {
            BaseTransformation transformation = new ConvertFromDateTransformation("dd/yyyy", true);
            DateTime input = new DateTime(2017, 1, 16, 14, 11, 56);
            object actual = transformation.TransformInputObject(input);
            object expected = "16/2017";
            Assert.AreEqual(expected, actual, "Convert from date transformation didn't correct date");
        }


        [ExpectedException(typeof(InvalidCastException))]
        [TestMethod]
        public void InvalidCastExceptionWorks() {
            BaseTransformation transformation = new ConvertFromDateTransformation("dd/MM/yyyy HH:mm:ss", false);
            string input = "xxxxx";
            object actual = transformation.TransformInputObject(input);
        }

        [TestMethod]
        public void SplitStringTransformationWorks() {
            BaseTransformation transformation = new SplitStringTransformation(";", false);
            string input = "test;test;";
            string[] actual = ((IEnumerable<string>)transformation.TransformInputObject(input)).ToArray();
            string[] expected = new string[] { "test", "test", "" };
            CollectionAssert.AreEqual(expected, actual, "Split string transformation didn't work correctly");
        }

        [TestMethod]
        public void SplitStringTransformationIgnoreMissingWorks() {
            BaseTransformation transformation = new SplitStringTransformation(";", true);
            string input = "test;test;";
            string[] actual = ((IEnumerable<string>)transformation.TransformInputObject(input)).ToArray();
            string[] expected = new string[] { "test", "test" };
            CollectionAssert.AreEqual(expected, actual, "Split string transformation ignore missing didn't work correctly");
        }

        [TestMethod]
        public void SearchStringTransformationWorks() {
            BaseTransformation transformation = new SearchStringTransformation(@"@(.+?\.com)", 1);
            string input = "test@test.com;test@domain.com";
            string[] actual = ((IEnumerable<string>)transformation.TransformInputObject(input)).ToArray();
            string[] expected = new string[] { "test.com", "domain.com" };
            CollectionAssert.AreEqual(expected, actual, "Search string transformation didn't work correctly");
        }
    }
}
