using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using edtools.Remap;
using edtools.Remap.Types;
using edtools.Remap.Mapping;
using edtools.Remap.Transformation;

namespace edtools.test.StructuredLoad {
    [TestClass]
    public class StructuredLoadFullMappingTests {
        [TestMethod]
        public void DirectMappingWorks() {
            DirectMapping mapper = new DirectMapping("X");
            DirectType formatter = new DirectType();
            BaseTransformation[] transformations = new BaseTransformation[] { };
            SingleMapping fullMapping = new SingleMapping(mapper, transformations, formatter);
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("X", "a");
            object expected = "a";
            object actual = fullMapping.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "Direct transformation didn't extract correct value");
        }

        [TestMethod]
        public void DateMappingWorks() {
            TemplateMapping mapper = new TemplateMapping("$[Date] $[Time]");
            BaseType formatter = new DateType("yyyy/MM/dd HH:mm:ss", false);
            BaseTransformation[] transformations = new BaseTransformation[] { };
            SingleMapping fullMapping = new SingleMapping(mapper, transformations, formatter);
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("Date", "2017/12/14");
            inputRow.Add("Time", "12:13:00");
            object expected = new DateTime(2017, 12, 14, 12, 13, 0);
            object actual = fullMapping.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "Date transformation didn't extract correct value");
        }

        [TestMethod]
        public void MultiValueMappingWorks() {
            DirectMapping mapper = new DirectMapping("A");
            BaseType formatter = new MultiValueType("|");
            BaseTransformation[] transformations = new BaseTransformation[] { };
            SingleMapping fullMapping = new SingleMapping(mapper, transformations, formatter);
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("A", "A|B|C|D");
            string[] expected = new string[] { "A", "B", "C", "D" };
            string[] actual = (string[])fullMapping.GetValue(inputRow);
            CollectionAssert.AreEqual(expected, actual, "Multi value transformation didn't extract correct value");
        }

        [TestMethod]
        public void WorksWithTransformations() {
            BaseMapping mapper = new ScriptMapping("return $inputValues.A");
            BaseTransformation[] transformations = new BaseTransformation[] { new ScriptTransformation("return $InputValue + \"|Script\"") };
            BaseType formatter = new MultiValueType("|");
            SingleMapping fullMapping = new SingleMapping(mapper, transformations, formatter);

            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("A", "A|B|C|D");
            string[] expected = new string[] { "A", "B", "C", "D", "Script" };
            string[] actual = (string[])fullMapping.GetValue(inputRow);
            CollectionAssert.AreEqual(expected, actual, "Didn't work with script transformation");
        }
    }
}
