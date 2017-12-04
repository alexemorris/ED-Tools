using System.Net.Mail;
using System.Collections.Generic;

namespace edtools.StructuredLoad {
    public class StructuredLoadEmail : StructuredLoadNode {

        public StructuredLoadEmail(Dictionary<string, object> entry) : base(entry) {
            this.EmailInfo = new MailMessage();
            EmailInfo.Subject = (string)entry["Subject"];
            this.EmailInfo.From = (MailAddress)entry["FROM"];

            ((List<MailAddress>)entry["TO"]).ForEach(x => this.EmailInfo.To.Add(x));
            ((List<MailAddress>)entry["CC"]).ForEach(x => this.EmailInfo.CC.Add(x));
            ((List<MailAddress>)entry["BCC"]).ForEach(x => this.EmailInfo.Bcc.Add(x));

        }

        public MailMessage EmailInfo { get; set; }
    }
}