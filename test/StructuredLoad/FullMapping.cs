using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using edtools.Remap;
using edtools.Remap.Mapping;
using edtools.Remap.Transformation;

namespace edtools.test.StructuredLoad {
    [TestClass]
    public class StructuredLoadFullMappingTests {
        [TestMethod]
        public void FullMappingWorks() {
            DirectMapping mapper = new DirectMapping("Y");
            BaseTransformation[] transformations = new BaseTransformation[] { };
            SingleMapping singleMapping = new SingleMapping(mapper, transformations);
            FullMapping fullMapping = new FullMapping();
            fullMapping.AddMapping("Z", singleMapping);
            Dictionary<string, object> inputRow = new Dictionary<string, object>();
            inputRow.Add("X", "a");
            inputRow.Add("Y", "b");
            Dictionary<string, object> expected = new Dictionary<string, object>();
            expected.Add("Z", "b");
            Dictionary<string, object> actual = fullMapping.GetRow(inputRow);
            CollectionAssert.AreEqual(expected, actual, "Full mapping didn't work as expected");
        }
    }
}
