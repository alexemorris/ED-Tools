using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using edtools.OPT;

namespace edtools.test.OPT {
    [TestClass]
    public class OPTTests {
        [TestMethod]
        public void ReadsEntrySuccessfully() {
            StringReader reader = new StringReader("0,,D:\\Output,Y,,,1");
            ParseOPT parser = new ParseOPT();
            List<OPTDocument> entries = new List<OPTDocument>();
            while (reader.Peek() >= 0) {
                OPTDocument current = parser.ReadLine(reader.ReadLine());
                if (current != null) {
                    entries.Add(current);
                }
            }
            OPTDocument final = parser.ReadEnd();
            if (final != null) {
                entries.Add(final);
            }
            OPTDocument entry = entries[0];

            string expected = "0";
            string actual = entry.Pages[0].ID;
            Assert.AreEqual(expected, actual, "Not parsed out correct ID");
        }

        [TestMethod]
        public void ReadsEntryCustomQuoteSuccessfully() {
            StringReader reader = new StringReader("'0','','D:\\Output','Y','','','1'");
            ParseOPT parser = new ParseOPT(quote: '\'');
            List<OPTDocument> entries = new List<OPTDocument>();
            while (reader.Peek() >= 0) {
                OPTDocument current = parser.ReadLine(reader.ReadLine());
                if (current != null) {
                    entries.Add(current);
                }
            }
            OPTDocument final = parser.ReadEnd();
            if (final != null) {
                entries.Add(final);
            }
            string expected = "D:\\Output";
            string actual = entries[0].Pages[0].Path;
            Assert.AreEqual(expected, actual, "Not parsed out correct path");
        }

        [TestMethod]
        public void ReadsEntryCustomDelimiterSuccessfully() {
            StringReader reader = new StringReader("'0'~''~'D:\\Output'~'Y'~''~''~'1'");
            ParseOPT parser = new ParseOPT(delimiter: '~', quote: '\'');
            List<OPTDocument> entries = new List<OPTDocument>();
            while (reader.Peek() >= 0) {
                OPTDocument current = parser.ReadLine(reader.ReadLine());
                if (current != null) {
                    entries.Add(current);
                }
            }
            OPTDocument final = parser.ReadEnd();
            if (final != null) {
                entries.Add(final);
            }
            OPTDocument entry = entries[0];
            string expected = "D:\\Output";
            string actual = entry.Pages[0].Path;
            Assert.AreEqual(expected, actual, "Not parsed out correct path");
        }

        [TestMethod]
        public void ReadsEntriesSuccessfully() {
            StringReader reader = new StringReader("0,,D:\\Output.tiff,Y,,,1\n0,,D:\\Output2.tiff,Y,,,1");
            ParseOPT parser = new ParseOPT();
            List<OPTDocument> entries = new List<OPTDocument>();
            while (reader.Peek() >= 0) {
                OPTDocument current = parser.ReadLine(reader.ReadLine());
                if (current != null) {
                    entries.Add(current);
                }
            }
            OPTDocument final = parser.ReadEnd();
            if (final != null) {
                entries.Add(final);
            }
            OPTDocument entry = entries[1];
            string expected = "D:\\Output2.tiff";
            string actual = entry.Pages[0].Path;
            Assert.AreEqual(expected, actual, "Not parsed out correct path");
        }

        [TestMethod]
        public void ReadsMultiPageEntriesSuccessfully() {
            StringReader reader = new StringReader("" +
                "0,,D:\\Output.tiff,Y,,,1\n" +
                "0,,D:\\Output2.tiff,Y,,,2\n" +
                "0,,D:\\Output3.tiff,,,,"
            );
            ParseOPT parser = new ParseOPT();
            List<OPTDocument> entries = new List<OPTDocument>();
            while (reader.Peek() >= 0) {
                OPTDocument output = parser.ReadLine(reader.ReadLine());
                if (output != null) {
                    entries.Add(output);
                }
            }
            OPTDocument final = parser.ReadEnd();
            if (final != null) {
                entries.Add(final);
            }
            OPTDocument entry = entries[1];
            string expected = "D:\\Output3.tiff";
            string actual = entry.Pages[1].Path;
            Assert.AreEqual(expected, actual, "Not parsed out correct path");
        }

        [TestMethod]
        public void ReadsCorrectCountOfMultiPageEntriesSuccessfully() {
            StringReader reader = new StringReader("" +
                "0,,D:\\Output.tiff,Y,,,1\n" +
                "1,,D:\\Output2.tiff,Y,,,2\n" +
                "2,,D:\\Output3.tiff,,,,\n" +
                "3,,D:\\Output4.tiff,Y,,,1\n" +
                "4,,D:\\Output5.tiff,Y,,,2\n" +
                "5,,D:\\Output6.tiff,,,,"
            );
            ParseOPT parser = new ParseOPT();
            List<OPTDocument> entries = new List<OPTDocument>();
            while (reader.Peek() >= 0) {
                OPTDocument output = parser.ReadLine(reader.ReadLine());
                if (output != null) {
                    entries.Add(output);
                }
            }
            OPTDocument final = parser.ReadEnd();
            if (final != null) {
                entries.Add(final);
            }
            int[] expected =  { 1, 2, 1, 2 };
            int[] actual = entries.Select(entry => entry.PageCount).ToArray();
            CollectionAssert.AreEqual(expected, actual, "Not parsed out number of pages for each document from multidoc OPT");
        }

    }
}
