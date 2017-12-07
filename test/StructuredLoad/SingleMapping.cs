using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using edtools.Remap;
using edtools.Remap.Mapping;
using edtools.Remap.Transformation;

namespace edtools.test.Remap {
    [TestClass]
    public class RemapSingleMappingTests {
        [TestMethod]
        public void DirectMappingWorks() {
            DirectMapping mapper = new DirectMapping("X");
            BaseTransformation[] transformations = new BaseTransformation[] { };
            SingleMapping fullMapping = new SingleMapping(mapper, transformations);
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("X", "a");
            object expected = "a";
            object actual = fullMapping.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "Direct transformation didn't extract correct value");
        }

        [TestMethod]
        public void DateMappingWorks() {
            TemplateMapping mapper = new TemplateMapping("$[Date] $[Time]");
            ConvertToDateTransformation dateTransform = new ConvertToDateTransformation("yyyy/MM/dd HH:mm:ss", false);
            BaseTransformation[] transformations = new BaseTransformation[] { dateTransform };
            SingleMapping fullMapping = new SingleMapping(mapper, transformations);
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("Date", "2017/12/14");
            inputRow.Add("Time", "12:13:00");
            object expected = new DateTime(2017, 12, 14, 12, 13, 0);
            object actual = fullMapping.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "Date transformation didn't extract correct value");
        }

        //[TestMethod]
        //public void MultiValueMappingWorks() {
        //    DirectMapping mapper = new DirectMapping("A");
        //    BaseType formatter = new BaseType("|");
        //    BaseTransformation[] transformations = new BaseTransformation[] { };
        //    SingleMapping fullMapping = new SingleMapping(mapper, transformations, formatter);
        //    Dictionary<string, object> inputRow = new Dictionary<string, object>();
        //    inputRow.Add("A", "A|B|C|D");
        //    string[] expected = new string[] { "A", "B", "C", "D" };
        //    string[] actual = (string[])fullMapping.GetValue(inputRow);
        //    CollectionAssert.AreEqual(expected, actual, "Multi value transformation didn't extract correct value");
        //}

    }
}
