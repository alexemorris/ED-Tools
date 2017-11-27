using System.Collections.Generic;
using System.Linq;

namespace edtools.OPT {
    public class OPTDocument {

        public OPTDocument() {
            this.Pages = new List<OPTPage>();
        }
        public void AddPage(OPTPage page) {
            this.Pages.Add(page);
        }

        public List<OPTPage> Pages { get; }
        public string ID {
            get { return this.Pages[0].ID; }
        }
        public int PageCount {
            get { return this.Pages.Count(); }
        }
        public string GetID(int i = 0) {
            return this.Pages[i].ID;
        }

        public string[] Stringify(string quote = null, string delimiter = ",") {
            return this.Pages.Select((page, index) => {
                if (index == 0) {
                    return page.Stringify(quote: quote, delimiter: delimiter, count: this.PageCount);
                }
                return page.Stringify(quote: quote, delimiter: delimiter, count: 0);
            }).ToArray();
        }
    }
}
