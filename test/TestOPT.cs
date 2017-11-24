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
            List<OPTEntry> entries = new List<OPTEntry>();
            while (reader.Peek() >= 0) {
                OPTEntry current = parser.readLine(reader.ReadLine());
                if (current != null) {
                    entries.Add(current);
                }
            }
            OPTEntry final = parser.readEnd();
            if (final != null) {
                entries.Add(final);
            }
            OPTEntry entry = entries[0];
            string expected = "D:\\Output";
            string actual = entry.GetPaths()[0];
            Assert.AreEqual(expected, actual, "Not parsed out correct path");
        }

        [TestMethod]
        public void ReadsEntryCustomQuoteSuccessfully() {
            StringReader reader = new StringReader("'0','','D:\\Output','Y','','','1'");
            ParseOPT parser = new ParseOPT(quote: '\'');
            List<OPTEntry> entries = new List<OPTEntry>();
            while (reader.Peek() >= 0) {
                OPTEntry current = parser.readLine(reader.ReadLine());
                if (current != null) {
                    entries.Add(current);
                }
            }
            OPTEntry final = parser.readEnd();
            if (final != null) {
                entries.Add(final);
            }
            OPTEntry entry = entries[0];
            string expected = "D:\\Output";
            string actual = entry.GetPaths()[0];
            Assert.AreEqual(expected, actual, "Not parsed out correct path");
        }

        [TestMethod]
        public void ReadsEntryCustomDelimiterSuccessfully() {
            StringReader reader = new StringReader("'0'~''~'D:\\Output'~'Y'~''~''~'1'");
            ParseOPT parser = new ParseOPT(delimiter: '~', quote: '\'');
            List<OPTEntry> entries = new List<OPTEntry>();
            while (reader.Peek() >= 0) {
                OPTEntry current = parser.readLine(reader.ReadLine());
                if (current != null) {
                    entries.Add(current);
                }
            }
            OPTEntry final = parser.readEnd();
            if (final != null) {
                entries.Add(final);
            }
            OPTEntry entry = entries[0];
            string expected = "D:\\Output";
            string actual = entry.GetPaths()[0];
            Assert.AreEqual(expected, actual, "Not parsed out correct path");
        }

        [TestMethod]
        public void ReadsEntriesSuccessfully() {
            StringReader reader = new StringReader("0,,D:\\Output.tiff,Y,,,1\n0,,D:\\Output2.tiff,Y,,,1");
            ParseOPT parser = new ParseOPT();
            List<OPTEntry> entries = new List<OPTEntry>();
            while (reader.Peek() >= 0) {
                OPTEntry current = parser.readLine(reader.ReadLine());
                if (current != null) {
                    entries.Add(current);
                }
            }
            OPTEntry final = parser.readEnd();
            if (final != null) {
                entries.Add(final);
            }
            OPTEntry entry = entries[1];
            string expected = "D:\\Output2.tiff";
            string actual = entry.GetPaths()[0];
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
            List<OPTEntry> entries = new List<OPTEntry>();
            while (reader.Peek() >= 0) {
                OPTEntry output = parser.readLine(reader.ReadLine());
                if (output != null) {
                    entries.Add(output);
                }
            }
            OPTEntry final = parser.readEnd();
            if (final != null) {
                entries.Add(final);
            }
            OPTEntry entry = entries[1];
            string expected = "D:\\Output3.tiff";
            string actual = entry.GetPaths()[1];
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
            List<OPTEntry> entries = new List<OPTEntry>();
            while (reader.Peek() >= 0) {
                OPTEntry output = parser.readLine(reader.ReadLine());
                if (output != null) {
                    entries.Add(output);
                }
            }
            OPTEntry final = parser.readEnd();
            if (final != null) {
                entries.Add(final);
            }
            int[] expected =  { 1, 2, 1, 2 };
            int[] actual = entries.Select(entry => entry.GetPageCount()).ToArray();
            CollectionAssert.AreEqual(expected, actual, "Not parsed out number of pages for each document from multidoc OPT");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
            "A false page count was missed.")]
        public void CorrectlyIdentifiesIncorrectPageCount() {
            StringReader reader = new StringReader("" +
                "0,,D:\\Output.tiff,Y,,,1\n" +
                "0,,D:\\Output2.tiff,Y,,,\n" +
                "0,,D:\\Output3.tiff,,,,2\n" +
                "0,,D:\\Output4.tiff,Y,,,1\n" +
                "0,,D:\\Output5.tiff,Y,,,1\n" +
                "0,,D:\\Output6.tiff,,,,"
            );
            ParseOPT parser = new ParseOPT();
            while (reader.Peek() >= 0) {
                OPTEntry output = parser.readLine(reader.ReadLine());
            }
        }
    }
}
