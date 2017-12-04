using System.IO;
using System.Collections.Generic;
using System;

namespace edtools.StructuredLoad {
     public class StructuredLoadFile : StructuredLoadNode {
        public FileInfo OriginalFileInfo { get; set; }
        public StructuredLoadFile(Dictionary<string, object> entry) : base(entry) {
            this.OriginalFileInfo = new FileInfo((string)entry["File Path"]);

            object creation = null;
            object modified = null;
            object access = null;

            entry.TryGetValue("Creation Date", out creation);
            entry.TryGetValue("Modified Date", out modified);
            entry.TryGetValue("Access Date", out access);

            this.OriginalFileInfo.CreationTime = (DateTime)creation;
            this.OriginalFileInfo.LastWriteTime = (DateTime)modified;
            this.OriginalFileInfo.LastAccessTime = (DateTime)access;

        }
    }
}

