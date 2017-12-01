using System.Linq;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System;
using edtools.OPT;

namespace edtools.StructuredLoad {
    public class StructuredLoadNode {
        public Tuple<string> BatesNumbers { get; set; }
        public Tuple<string> AttachmentRange { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public HashAlgorithm Hash { get; set; }
        public StructuredLoadNode Parent { get; set; }
        public StructuredLoadNode Root { get; set; }
        public StructuredLoadNode[] Children { get; set; }
        public FileInfo NativeLink { get; set; }
        public FileInfo TextLink { get; set; }
        public FileInfo ImageLink { get; set; }
        public MailAddress[] EmailTo { get; set; }
        public MailAddress[] EmailFrom { get; set; }
        public MailAddress[] EmailCC { get; set; }
        public MailAddress[] EmailBCC { get; set; }
        public FileInfo OriginalFileInfo { get; set; }
        public OPTDocument OPTDocument { get; set; }
    }
}
