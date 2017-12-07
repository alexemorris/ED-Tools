using Microsoft.VisualStudio.TestTools.UnitTesting;
using edtools.StructuredLoad;
using System.Collections.Generic;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System;

namespace edtools.test.StructuredLoad {
    [TestClass]
    public class StructuredLoadNodeTests {
        private static Random random = new Random();
        public Dictionary<string, object> MockSLNode(int index = 10) {
            Dictionary<string, object> inputData = new Dictionary<string, object>();
            inputData.Add("ID", "XXX" + index.ToString());
            inputData.Add("Bates Begin", "AAA" + (index.ToString()));
            inputData.Add("Bates End", "AAA" + (index + 2).ToString());
            inputData.Add("Attach Begin", (index.ToString()));
            inputData.Add("Attach End", (index + 5).ToString());
            inputData.Add("Native Link", new FileInfo("C:/" + "AAA" + (index.ToString()) + ".pdf"));
            inputData.Add("Text Link", new FileInfo("C:/" + "AAA" + (index.ToString()) + ".txt"));
            return inputData;
        }

        public DateTime RandomDate(int year) {
            int day = random.Next(1, 30);
            int month = random.Next(1, 12);
            int hour = random.Next(0, 23);
            int minute = random.Next(0, 60);
            int second = random.Next(0, 60);
            return new DateTime(year, month, day, hour, minute, second);
        }

        public string RandomString(int length) {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public MailAddress MockEmail() {
            string to = RandomString(10);
            string domain = RandomString(10);
            string[] suffixes = new string[] { ".com", ".co.uk" };
            string suffix = suffixes[random.Next(suffixes.Length)];
            return new MailAddress(to + '@' + domain + suffix);
        }

        public List<MailAddress> MockEmails(int min, int max) {
            return Enumerable.Repeat("", random.Next(min, max)).Select(x => MockEmail()).ToList();
        }

        public Dictionary<string, object> MockSLEmail(int index = 10) {
            Dictionary<string, object> inputData = MockSLNode(index);
            inputData.Add("TO", MockEmails(0,10));
            inputData.Add("CC", MockEmails(0, 20));
            inputData.Add("BCC", MockEmails(0, 5));
            inputData.Add("FROM", MockEmail());
            inputData.Add("Subject", RandomString(20));

            DateTime sent = RandomDate(2017);
            DateTime recieved = sent;
            recieved.AddSeconds(10);

            inputData.Add("Sent Date", sent);
            inputData.Add("Recieved Date", recieved);
            return inputData;
        }

        public Dictionary<string, object> MockSLFile(int index = 10) {
            Dictionary<string, object> inputData = MockSLNode(index);

            DateTime created = RandomDate(2017);

            DateTime modified = created;
            modified.AddDays(10);
            DateTime access = modified;

            inputData.Add("Created Date", created);
            inputData.Add("Modified Date", modified);
            inputData.Add("Access Date", access);
            inputData.Add("File Path", RandomString(10));

            return inputData;
        }

        [TestMethod]
        public void NodeConstructorWorks() {
            Dictionary<string, object> inputData = MockSLNode(10);
            StructuredLoadNode transformation = new StructuredLoadNode(inputData);
            string actualID = transformation.ID;
            string expectedID = (string)inputData["ID"];
            Assert.AreEqual(expectedID, actualID, "Structured load node constructor didn't get ID correctly");

            string actualBatesBegin = transformation.BatesRange.Item1;
            string expectedBatesBegin = (string)inputData["Bates Begin"];
            Assert.AreEqual(expectedBatesBegin, actualBatesBegin, "Structured load node constructor didn't get bates begin correctly");

        }

        [TestMethod]
        public void NodeConstructorWorksWithDatePrinted() {
            Dictionary<string, object> inputData = MockSLNode(7);
            StructuredLoadNode transformation = new StructuredLoadNode(inputData);
            DateTime printed = RandomDate(2017);
            inputData.Add("Date Printed", printed);
            string actualID = transformation.ID;
            string expectedID = (string)inputData["ID"];
            Assert.AreEqual(expectedID, actualID, "Structured load node constructor didn't get ID correctly");

            string actualBatesBegin = transformation.BatesRange.Item1;
            string expectedBatesBegin = (string)inputData["Bates Begin"];
            Assert.AreEqual(expectedBatesBegin, actualBatesBegin, "Structured load node constructor didn't get bates begin correctly");
            Assert.AreEqual(printed, transformation.PrintedDate, "Structured load node constructor didn't get date printed correctly");
        }

        [TestMethod]
        public void EmailConstructorWorks() {
            Dictionary<string, object> inputData = MockSLEmail(7);
            StructuredLoadEmail transformation = new StructuredLoadEmail(inputData);
            DateTime actualsent = transformation.SentDate;
            DateTime expectedsent = (DateTime)inputData["Sent Date"];
            Assert.AreEqual(expectedsent, actualsent, "Structured load email node constructor didn't get sent date correctly");

            MailAddress actualTo = transformation.EmailInfo.To[0];
            MailAddress expectedTo = ((List<MailAddress>)inputData["TO"])[0];
            Assert.AreEqual(expectedTo, actualTo, "Structured load node constructor didn't get to correctly");
        }

        [TestMethod]
        public void FileConstructorWorks() {
            Dictionary<string, object> inputData = MockSLFile(7);
            StructuredLoadFile transformation = new StructuredLoadFile(inputData);
            DateTime actualmodified = transformation.LastWriteTime;
            DateTime expectedmodified = (DateTime)inputData["Modified Date"];
            Assert.AreEqual(expectedmodified, actualmodified, "Structured load file node constructor didn't get modified date correctly");

            string actualFile = transformation.OriginalFilePath;
            string expectedFile = (string)inputData["File Path"];
            Assert.AreEqual(expectedFile, actualFile, "Structured load file constructor didn't get file name correctly");
        }


    }
}
