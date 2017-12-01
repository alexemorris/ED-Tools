using System;
using System.Globalization;

namespace edtools.Remap.Types {
    public class DateType : BaseType {
        string[] dateFormats;
        bool ignoreInvalid;
        public DateType(string dateFormat, bool ignoreInvalid = true) {
            this.dateFormats = new string[] { dateFormat };
            this.ignoreInvalid = ignoreInvalid;
        }

        public DateType(string[] dateFormats, bool ignoreInvalid = true) {
            this.dateFormats = dateFormats;
            this.ignoreInvalid = ignoreInvalid;
        }
        public override object CastValue(string inputValue) {
            foreach(string dateFormat in dateFormats) {
                try {
                    DateTime result = DateTime.ParseExact(inputValue, dateFormat, new CultureInfo("en-US"));
                    return result;
                } catch (FormatException) {
                    continue;
                }
            }
            if (ignoreInvalid) {
                return null;
            } else {
                throw new FormatException("No date formats matched input data");
            }
        }
    }
}
