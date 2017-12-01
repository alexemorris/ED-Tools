using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using edtools.Remap.Types;

namespace edtools.test.StructuredLoad {
    [TestClass]
    public class StructuredLoadFormatterTests {
        [TestMethod]
        public void BaseFormatterWorksCorrectly() {
            DirectType direct = new DirectType();
            string expected = "supercalifragilisticexpialidocious";
            object actual = direct.CastValue(expected);
            Assert.AreEqual(expected, actual, "Direct formatter didn't format value correctly");
        }

        [TestMethod]
        public void MultiValueFormatterWorksCorrectly() {
            MultiValueType multiValueFormatter = new MultiValueType("[delimiter]");
            string input = "test[delimiter]test2[delimiter][delimiter]";
            string[] expected = new string[] { "test", "test2"};
            string[] actual = (string[])multiValueFormatter.CastValue(input);
            CollectionAssert.AreEqual(expected, actual, "Not parsed out multivalue formatter correctly");
        }

        [TestMethod]
        public void MultiValueFormatterWorksCorrectlyWithNoRemoveBlanks() {
            MultiValueType multiValueFormatter = new MultiValueType("[delimiter]", false);
            string input = "test[delimiter]test2[delimiter]";
            string[] expected = new string[] { "test", "test2", "" };
            string[] actual = (string[])multiValueFormatter.CastValue(input);
            CollectionAssert.AreEqual(expected, actual, "Not parsed out multivalue formatter correctly without removing blanks");
        }

        [TestMethod]
        public void DateFormatterWorksCorrectly() {
            DateType dateFormatter = new DateType("yyyy-MM-dd");
            string input = "2017-01-12";
            DateTime? expected = new DateTime(2017, 1, 12);
            DateTime? actual = (DateTime)dateFormatter.CastValue(input);
            Assert.AreEqual(expected, actual, "Not parsed out date correctly with date formatter");
        }

        [TestMethod]
        public void DateFormatterWorksCorrectlyWithInvalidDates() {
            DateType dateFormatter = new DateType("yyyy-MM-dd");
            string input = "2017-0112";
            DateTime? expected = null;
            DateTime? actual = (DateTime?)dateFormatter.CastValue(input);
            Assert.AreEqual(expected, actual, "Not parsed out date correctly with date formatter");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
            "No date formats matched input data")]

        public void DateFormatterWorksThrowsWithInvalidDates() {
            DateType dateFormatter = new DateType("yyyy-MM-dd", false);
            string input = "2017-0112";
            DateTime? actual = (DateTime?)dateFormatter.CastValue(input);
        }
    }
}
