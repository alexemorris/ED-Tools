using System;
using System.Globalization;

namespace edtools.Remap.Transformation {
    public class ConvertFromDateTransformation : BaseTransformation {
        string dateOutputFormat;
        bool ignoreInvalid;
        CultureInfo culture = new CultureInfo("en-US");
        public ConvertFromDateTransformation(string dateOutputFormat, bool ignoreInvalid = true) {
            this.dateOutputFormat = dateOutputFormat;
            this.ignoreInvalid = ignoreInvalid;
        }


        public override object TransformSingleObject(object inputObject) {
            DateTime inputDate = CheckType<DateTime>(inputObject);
            string output = null;
            try {
                output = inputDate.ToString(dateOutputFormat);
            } catch (FormatException) {
                if (ignoreInvalid) {
                    return null;
                } else {
                    throw new FormatException(String.Format("Cannot cast datetime object to specified format {0}", dateOutputFormat));
                }
            }
            return output;
        }

    }
}
