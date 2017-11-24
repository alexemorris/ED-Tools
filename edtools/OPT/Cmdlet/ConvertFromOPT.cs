using System.Management.Automation;

namespace edtools.OPT {
    class ConvertFromOPT : Cmdlet {

        // Declare the parameters for the cmdlet.
        [Parameter(Mandatory = true)]
        public string InputString {
            get { return inputString; }
            set { inputString = value; }
        }
        private string inputString;

        // Overide the ProcessRecord method to process
        // the supplied user name and write out a 
        // greeting to the user by calling the WriteObject
        // method.
        protected override void ProcessRecord() {
            
        }


    }
}
