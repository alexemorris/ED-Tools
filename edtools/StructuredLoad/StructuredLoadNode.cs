﻿using System.IO;
using System;
using System.Collections.Generic;
using edtools.OPT;

namespace edtools.StructuredLoad {
    public class StructuredLoadNode { 
        public string ID { get; set; }
        public Tuple<string, string> BatesRange { get; set; }
        public Tuple<string, string> AttachRange { get; set; }
        public string Hash { get; set; }
        public string Title { get; set; }
        public StructuredLoadNode Parent { get; set; }
        public List<StructuredLoadNode> Children { get; set; }
        public FileInfo NativeInfo { get; set; }
        public FileInfo TextInfo { get; set; }
        public FileInfo ImageInfo { get; set; }
        public DateTime PrintedDate { get; set; }
        public OPTDocument OPTDocument { get; set; }
        public Dictionary<string, object> Fields { get; set; }

        public StructuredLoadNode(Dictionary<string, object> entry) {

            this.Fields = entry;
            this.ID = (string)entry["ID"];
            this.BatesRange = new Tuple<string, string>((string)entry["Bates Begin"], (string)entry["Bates End"]);
            this.AttachRange = new Tuple<string, string>((string)entry["Attach Begin"], (string)entry["Attach End"]);

            if (entry.TryGetValue("Image Info", out object imageInfo)) {
                this.ImageInfo = (FileInfo)imageInfo;
            }

            this.NativeInfo = (FileInfo)entry["Native Link"];
            this.TextInfo = (FileInfo)entry["Text Link"];

            if (entry.TryGetValue("Printed Date", out object printed)) {
                this.PrintedDate = (DateTime)printed;
            }          
        }
    }
}