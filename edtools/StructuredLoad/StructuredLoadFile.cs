using System.IO;
using System.Collections.Generic;
using System;

namespace edtools.StructuredLoad {
     public class StructuredLoadFile : StructuredLoadNode {
        public string OriginalFilePath { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastAccessTime { get; set; }

        public StructuredLoadFile(Dictionary<string, object> entry) : base(entry) {

            this.OriginalFilePath = (string)entry["File Path"];

            object creation = null;
            object modified = null;
            object access = null;

            entry.TryGetValue("Created Date", out creation);
            entry.TryGetValue("Modified Date", out modified);
            entry.TryGetValue("Access Date", out access);

            this.CreationTime = (DateTime)creation;
            this.LastWriteTime = (DateTime)modified;
            this.LastAccessTime = (DateTime)access;

        }
    }
}

