using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using PsUtils;

namespace edtools.IDX {
    public class ParseIDX {
        private static string documentDelimiter = "#DREENDDOC";
        private static Regex blockRegex = new Regex("#DRE.+?(?=#DRE)", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex[] compiledFieldRegex = {
            new Regex("FIELD (?<name>.+?)=\"(?<value>.*?)\"\r?\n?$", RegexOptions.Singleline | RegexOptions.Compiled),
            new Regex("#(?<name>.+?)( |\r?\n)(?<value>.+?)\r?\n?$", RegexOptions.Singleline | RegexOptions.Compiled),
        };
        private List<string> currentBlock;


        public ParseIDX() {    
            this.currentBlock = new List<string>();
        }

        public static Dictionary<string, object> ParseBlock(string doc) {
            var blockMatches = blockRegex.Matches(doc);
            var blocks = blockMatches.Cast<Match>().Select(match => match.Value).ToList();
            var docFields = new Dictionary<string, object>();
            if (blocks.Count > 0) {
                var processed = 0;
                foreach (var block in blocks) {
                    processed++;
                    Match match;
                    var i = 0;
                    do {
                        match = compiledFieldRegex[i].Match(block);
                        ++i;
                    } while (!match.Success && i < compiledFieldRegex.Length);

                    if (match.Success) {

                        var fieldName = match.Groups["name"].Value;
                        var fieldValue = match.Groups["value"].Value;

                        object existingField;

                        if (!docFields.TryGetValue(fieldName, out existingField)) {
                            docFields[fieldName] = fieldValue;
                        } else if (existingField is List<object>) {
                            var feList = (List<object>)existingField;
                            feList.Add(fieldValue);
                        } else {
                            docFields[fieldName] = new List<object> { existingField, fieldValue };
                        }
                    }
                }
            }
            return docFields;
        } 

        public Dictionary<string, object> ReadLine(string line) {
            if (line.StartsWith(documentDelimiter)) {
                string doc = String.Join("\n", currentBlock);
                currentBlock = new List<string>();
                return ParseBlock(doc);
            }
            currentBlock.Add(line);
            return null;
        }

        public static IEnumerable<byte[]> splitIDX(Stream input, Encoding encoding) {
            return StreamSplitter.Split(input, encoding.GetBytes(documentDelimiter), 128 * 1024);
        }

        public Dictionary<string, object> ReadEnd() {
            string doc = String.Join("\n", currentBlock);
            return ParseBlock(doc);
        }
    }
}
