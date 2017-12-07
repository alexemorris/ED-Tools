using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using edtools.Remap.Mapping;
using System.Linq;
namespace edtools.test.Remap {
    [TestClass]
    public class RemapMappingTests {
        [TestMethod]
        public void DirectMappingWorksCorrectly() {
            DirectMapping direct = new DirectMapping("X");
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("X", "a");
            inputRow.Add("Y", "b");
            object expected = "a";
            object actual = direct.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "Direct mapping didn't extract correct value");
        }

        [TestMethod]
        public void DirectMappingWorksCorrectlyWhenBlank() {
            DirectMapping direct = new DirectMapping("Z");
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("X", "a");
            inputRow.Add("Y", "b");
            object expected = "";
            object actual = direct.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "Direct mapping didn't extract null when couldn't find value");
        }

        [TestMethod]
        public void TemplateMappingWorksCorrectly() {
            TemplateMapping direct = new TemplateMapping("hello-$[A]");
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("A", "C");
            inputRow.Add("B", "D");
            object expected = "hello-C";
            object actual = direct.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "Template mapping didn't replace values correctly");
        }

        [TestMethod]
        public void TemplateMappingWorksCorrectlyIfKeyDoesntExist() {
            TemplateMapping template = new TemplateMapping("hello-$[C]");
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("A", "C");
            inputRow.Add("B", "D");
            object expected = "hello-$[C]";
            object actual = template.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "Template mapping didn't replace incorrect values correctly");
        }

        [TestMethod]
        public void FirstAvailableMappingWorksCorrectly() {
            FirstAvailableMapping template = new FirstAvailableMapping(new string[] { "F", "G", "A" });
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("A", "B");
            inputRow.Add("C", "D");
            inputRow.Add("E", "F");
            object expected = "B";
            object actual = template.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "First available mapping didn't get values correctly");
        }
        [TestMethod]
        public void FirstAvailableMappingWorksCorrectlyWhenDoesntExist() {
            FirstAvailableMapping template = new FirstAvailableMapping(new string[] { "F", "G", "A" });
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("K", "B");
            inputRow.Add("C", "D");
            inputRow.Add("E", "F");
            object expected = null;
            object actual = template.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "First available mapping didn't get values correctly");
        }


        [TestMethod]
        public void TemplateMappingWorksCorrectlyWithMultipleSubstitutions() {
            TemplateMapping template = new TemplateMapping("hello $[A] & $[B]");
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("A", "C");
            inputRow.Add("B", "D");
            object expected = "hello C & D";
            object actual = template.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "Template mapping work correctly for multiple values");
        }

        [TestMethod]
        public void ScriptMappingWorksCorrectly() {
            ScriptMapping script = new ScriptMapping("return \"hello \" + $inputValues.A");
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("A", "C");
            object expected = "hello C";
            object actual = script.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "Script mapping work correctly");
        }

        [TestMethod]
        public void ComplexScriptMappingWorksCorrectly() {
            ScriptMapping script = new ScriptMapping("return Get-Date ([datetime]::ParseExact($inputValues.WeirdDate,'ddMMyyyy_HHmmss',$null)) -Format 'yyyy-mm-dd'");
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("WeirdDate", "24122014_022257");
            string expected = "2014-22-24";
            object actual = script.GetValue(inputRow);
            Assert.AreEqual(expected, actual, "Complex script mapping work correctly");
        }

        [TestMethod]
        public void MultirowScriptMappingWorksCorrectly() {
            ScriptMapping script = new ScriptMapping("return Get-Date ([datetime]::ParseExact($inputValues.WeirdDate,'ddMMyyyy_HHmmss',$null)) -Format 'yyyy-mm-dd'");
            Dictionary<string, object> inputRow1 = new Dictionary<string, object>();
            inputRow1.Add("WeirdDate", "24122014_022257");
            string expected1 = "2014-22-24";
            string actual1 = (string)script.GetValue(inputRow1);

            Dictionary<string, object> inputRow2 = new Dictionary<string, object>();
            inputRow2.Add("WeirdDate", "24122017_022257");
            string expected2 = "2017-22-24";
            string actual2 = (string)script.GetValue(inputRow2);

            Dictionary<string, object> inputRow3 = new Dictionary<string, object>();
            inputRow3.Add("WeirdDate", "24122020_022257");
            string expected3 = "2020-22-24";
            string actual3 = (string)script.GetValue(inputRow3);

            CollectionAssert.AreEqual(
                new string[] { expected1, expected2, expected3 },
                new string[] { actual1, actual2, actual3 },
                "Wrong values parsed out for multi row script mapping"
            );

        }
    }
}
