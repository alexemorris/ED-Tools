Describe 'ConvertFrom-OPT' {
	 It "Given blank string it complains" {

		 try {
			"" | ConvertFrom-OPT
		 } catch {
			 $_ 
		 }
		
	  }
}