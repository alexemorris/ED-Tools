using System;
using System.Globalization;

namespace edtools.Remap.Transformation {
    public class ConvertToDateTransformation : BaseTransformation {
        string[] dateInputFormats;
        bool ignoreInvalid;
        CultureInfo culture = new CultureInfo("en-US");
        public ConvertToDateTransformation(string dateInputFormat, bool ignoreInvalid = true) {
            this.dateInputFormats = new string[] { dateInputFormat };
            this.ignoreInvalid = ignoreInvalid;
        }

        public ConvertToDateTransformation(string[] dateInputFormats, bool ignoreInvalid = true) {
            this.dateInputFormats = dateInputFormats;
            this.ignoreInvalid = ignoreInvalid;
        }

        public override object TransformSingleObject(object inputObject) {
            string inputString = CheckType<string>(inputObject);

            foreach (string dateFormat in dateInputFormats) {
                try {
                    DateTime result = DateTime.ParseExact(inputString, dateFormat, culture);
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
