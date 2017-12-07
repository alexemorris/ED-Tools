using System.Net.Mail;
using System.Collections.Generic;
using System;
using System.IO;
namespace edtools.StructuredLoad {
    public class StructuredLoadEmail : StructuredLoadNode {

        public StructuredLoadEmail(Dictionary<string, object> entry) : base(entry) {
            this.EmailInfo = new MailMessage();
            EmailInfo.Subject = (string)entry["Subject"];
            this.EmailInfo.From = (MailAddress)entry["FROM"];
          
            ((List<MailAddress>)entry["TO"]).ForEach(x => this.EmailInfo.To.Add(x));
            ((List<MailAddress>)entry["CC"]).ForEach(x => this.EmailInfo.CC.Add(x));
            ((List<MailAddress>)entry["BCC"]).ForEach(x => this.EmailInfo.Bcc.Add(x));

            if (entry.TryGetValue("Sent Date", out object sent)) {
                this.SentDate = (DateTime)sent;
            }
           
            if (entry.TryGetValue("Recieved Date", out object recieved)) {
                this.RecievedDate = (DateTime)recieved;
                if (this.RecievedDate < this.SentDate) {
                    throw new InvalidDataException(String.Format("Recieved date {0} cannot be before the sent date {1}", this.RecievedDate.ToLongDateString(), this.SentDate.ToLongDateString()));
                }
            }
            

        }

        public MailMessage EmailInfo { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime RecievedDate { get; set; }

    }
}