using System.IO;
using System.Linq;
using System.Collections.Generic;
using edtools.Delimited;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edtools.test.Concordance {
    [TestClass]
    public class ConcordanceTests {
        [TestMethod]
        public void ReadsEntrySuccessfully() {
            StringReader reader = new StringReader("þ0þþ1þþ2þ");
            ParseDelimited parser = new ParseDelimited(header: new string[] { "a", "b", "c" });
            List<Dictionary<string, object>> entries = new List<Dictionary<string, object>>();
            while (reader.Peek() >= 0) {
                entries.Add(parser.ReadLine(reader.ReadLine()));
            }
            Dictionary<string, object> entry = entries[0];
            string expected = "0";
            string actual = entry["a"].ToString();
            Assert.AreEqual(expected, actual, "Not parsed out correct input for column 1");
        }

        [TestMethod]
        public void ReadsEntriesSuccessfully() {
            StringReader reader = new StringReader("þ0þþ1þþ2þ\nþ3þþ4þþ5þ");
            ParseDelimited parser = new ParseDelimited(header: new string[] { "a", "b", "c" });
            List<Dictionary<string, object>> entries = new List<Dictionary<string, object>>();
            while (reader.Peek() >= 0) {
                entries.Add(parser.ReadLine(reader.ReadLine()));
            }
            string[] expected = { "0", "3" };
            string[] actual = entries.Select(row => row["a"].ToString()).ToArray();
            CollectionAssert.AreEqual(expected, actual, "Not parsed out correct columns for multirow concordance");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
            "Entry at line 0 has additional quote chars (þ) (found 5, expected 6). Ignoring line.")]
        public void ReadsInvalidEntriesSuccessfully() {
            StringReader reader = new StringReader("þ0þþ1þ2þ\nþ3þþ4þþ5þ");
            ParseDelimited parser = new ParseDelimited(header: new string[] { "a", "b", "c" });
            List<Dictionary<string, object>> entries = new List<Dictionary<string, object>>();
            while (reader.Peek() >= 0) {
                entries.Add(parser.ReadLine(reader.ReadLine()));
            }
        }

        [TestMethod]
        public void ReadsEntriesSuccessfullyCustomDelimiter() {
            StringReader reader = new StringReader("þ0þ,þ1þ,þ2þ\nþ3þ,þ4þ,þ5þ");
            ParseDelimited parser = new ParseDelimited(header: new string[] { "a", "b", "c" }, delimiter: ',');
            List<Dictionary<string, object>> entries = new List<Dictionary<string, object>>();
            while (reader.Peek() >= 0) {
                entries.Add(parser.ReadLine(reader.ReadLine()));
            }
            string[] expected = { "0", "3" };
            string[] actual = entries.Select(row => row["a"].ToString()).ToArray();
            CollectionAssert.AreEqual(expected, actual, "Not parsed out correct columns for multirow concordance");
        }

        [TestMethod]
        public void ReadsEntriesSuccessfullyDuplicateHeader() {
            StringReader reader = new StringReader("þ0þ,þ1þ,þ2þ,þ3þ\nþ4þ,þ5þ,þ6þ,þ7þ");
            ParseDelimited parser = new ParseDelimited(header: new string[] { "a", "b", "c", "a" }, delimiter: ',');
            List<Dictionary<string, object>> entries = new List<Dictionary<string, object>>();
            while (reader.Peek() >= 0) {
                entries.Add(parser.ReadLine(reader.ReadLine()));
            }
            string[] expected = { "3", "7" };
            string[] actual = entries.Select(row => row["a_2"].ToString()).ToArray();
            CollectionAssert.AreEqual(expected, actual, "Not parsed out correct columns for duplicate header concordance");
        }

        [TestMethod]
        public void ReadsHeaderFromFirstRow() {
            StringReader reader = new StringReader("þaþ,þbþ,þcþ,þdþ\nþ0þ,þ1þ,þ2þ,þ3þ\nþ4þ,þ5þ,þ6þ,þ7þ");
            ParseDelimited parser = new ParseDelimited(delimiter: ',');
            parser.ReadHeader(reader.ReadLine());
            List<Dictionary<string, object>> entries = new List<Dictionary<string, object>>();
            while (reader.Peek() >= 0) {
                entries.Add(parser.ReadLine(reader.ReadLine()));
            }
            string[] expected = { "3", "7" };
            string[] actual = entries.Select(row => row["d"].ToString()).ToArray();
            CollectionAssert.AreEqual(expected, actual, "Not parsed out correct data when header provided from concordance");
        }

    }
}
