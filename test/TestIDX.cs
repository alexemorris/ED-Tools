using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using edtools.IDX;
using System.Collections.Generic;

namespace edtools.test.IDX {
    [TestClass]
    public class IDXTests {
        [TestMethod]
        public void ReadsEntrySuccessfully() {
            StringReader reader = new StringReader("#DREREFERENCE a\n#DRETITLE b\n#DREFIELD A=\"c\"\n#DREFIELD B=\"d\"\n#DREFIELD C=\"e\"");
            ParseIDX parser = new ParseIDX();
            List<Dictionary<string, object>> entries = new List<Dictionary<string, object>>();
            while (reader.Peek() >= 0) {
                Dictionary<string, object> current = parser.ReadLine(reader.ReadLine());
                if (current != null) {
                    entries.Add(current);
                }
            }
            Dictionary<string, object> final = parser.ReadEnd();
            if (final != null) {
                entries.Add(final);
            }
            Dictionary<string, object> entry = entries[0];
            string expected = "b";
            string actual = entry["DRETITLE"].ToString();
            Assert.AreEqual(expected, actual, "Not parsed out correct input for  DRETITLE");
        }

        [TestMethod]
        public void ReadsEntriesSuccessfully() {
            StringReader reader = new StringReader("#DREREFERENCE a\n#DRETITLE b\n#DREFIELD A=\"c\"\n#DREFIELD B=\"d\"\n#DREFIELD C=\"e\"\n#DREENDDOC\n#DREREFERENCE f\n#DRETITLE g\n#DREFIELD A=\"h\"\n#DREFIELD B=\"i\"\n#DREFIELD C=\"j\"#DREENDDOC");
            ParseIDX parser = new ParseIDX();
            List<Dictionary<string, object>> entries = new List<Dictionary<string, object>>();
            while (reader.Peek() >= 0) {
                Dictionary<string, object> current = parser.ReadLine(reader.ReadLine());
                if (current != null) {
                    entries.Add(current);
                }
            }
            Dictionary<string, object> final = parser.ReadEnd();
            if (final != null) {
                entries.Add(final);
            }
            Dictionary<string, object> entry = entries[1];
            string expected = "h";
            string actual = entry["A"].ToString();
            Assert.AreEqual(expected, actual, "Not parsed out correct input for field in second document");

        }
    }
}
