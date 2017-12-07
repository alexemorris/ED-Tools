using System.Net.Mail;
using System;

namespace edtools.Remap.Transformation {
    public class MailAddressTransformation : BaseTransformation {

        bool ignoreInvalid;
        public MailAddressTransformation(bool ignoreInvalid = false) {
            this.ignoreInvalid = ignoreInvalid;
        }
        public override object TransformSingleObject(object inputObject) {
            string input = CheckType<string>(inputObject);
            try {
                return new MailAddress(input);
            } catch (FormatException) {
                if (!ignoreInvalid) {
                    throw new FormatException(string.Format("Email {0} not in the correct format, using ignore invalid flag to ignore incorrect emails", input));
                }
                return null;
            } catch (ArgumentException) {
                if (!ignoreInvalid) {
                    throw new ArgumentException(string.Format("Email {0} not in the correct format, using ignore invalid flag to ignore incorrect emails", input));
                }
                return null;
            }
        }
    }
}
