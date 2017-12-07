# ED-Tools
Some incredibly useful parsers for importing, remapping and exporting various common datatypes in eDiscovery workflows (Concordance, Opticon, IDX). Includes various C# Powershell Cmdlets.

https://www.sec.gov/divisions/enforce/datadeliverystandards.pdf


## Installation instructions
Open the solution in a recent version of Visual Studio (developed on VS2017 Community). Ensure references are configured correctly. Currently there are 2 dependencies.

- System.Management.Automation - which is required for Powershell integration, the latest version of which can be acquired from NuGet.

- PSUtils - binary is included and includes some handy helper functions for splitting streams and converting Encoding from a Powershell enum to a System.Text.Encoding object.
Source code in the following repo:
https://github.com/antonyoni/PsUtils


Once up and running you can build the solution using the flag,  you can then use the output dll in directly powershell via some handy cmdlet wrappers.

```powershell
  Import-Module edtools.dll
  Import-Concordance "file.dat" -Verbose
  Get-Content "file.dat" | ConvertFrom-Concordance
  Import-OPT "images.opt"
  Import-IDX "big.idx" -Parallel
```

## Packaging full release module
In the future this should be automated. But its a 3 step process.
1. Build the solution using Release configuration
2. Create a *copy* of the 'powershell' directory in the root of the repository
3. Set the relevant version in the .psd1 file
4. You're good to go!

## Cmdlets
* Concordance Data
  * Import-Concordance
  * Import-ConcordanceHeader
  * Export-Concordance
  * ConvertFrom-Concordance
  * ConvertTo-Concordance

* Opticon Data
  * Import-OPT
  * Export-OPT
  * ConvertFrom-OPT
  * ConvertTo-OPT

* IDX Data
  * Import-IDX
  * ConvertFrom-IDX


## Remapping
*Still in development!*
.NET classes and corresponding Cmdlets that can remap input data columns, parse dates, delimited fields and email data.
# ED-Tools
Some incredibly useful parsers for importing, remapping and exporting various common datatypes in eDiscovery workflows (Concordance, Opticon, IDX). Includes various C# Powershell Cmdlets.

https://www.sec.gov/divisions/enforce/datadeliverystandards.pdf


## Installation instructions
Open the solution in a recent version of Visual Studio (developed on VS2017 Community). Ensure references are configured correctly. Currently there are 2 dependencies.

- System.Management.Automation - which is required for Powershell integration, the latest version of which can be acquired from NuGet.

- PSUtils - binary is included and includes some handy helper functions for splitting streams and converting Encoding from a Powershell enum to a System.Text.Encoding object.
Source code in the following repo:
https://github.com/antonyoni/PsUtils


Once up and running you can build the solution using the flag,  you can then use the output dll in directly powershell via some handy cmdlet wrappers.

```powershell
  Import-Module edtools.dll
  Import-Concordance "file.dat" -Verbose
  Get-Content "file.dat" | ConvertFrom-Concordance
  Import-OPT "images.opt"
  Import-IDX "big.idx" -Parallel
```

## Packaging full release module
In the future this should be automated. But its a 3 step process.
1. Build the solution using Release configuration
2. Create a *copy* of the 'powershell' directory in the root of the repository
3. Set the relevant version in the .psd1 file
4. You're good to go!

## Parsers
* Concordance Data
  * Import-Concordance
  * Export-Concordance
  * ConvertFrom-Concordance
  * ConvertTo-Concordance

* Opticon Data
  * Import-OPT
  * Export-OPT
  * ConvertFrom-OPT
  * ConvertTo-OPT

* IDX Data
  * Import-IDX
  * ConvertFrom-IDX


## Remapping
*Still in development!*
.NET classes and corresponding Cmdlets that can remap input data columns, parse dates, delimited fields and email data.

Create mapping.json file containing a list of all mappings & transformations. Below is a list of currently available options.

* Mapping
  * DirectMapping - Takes data from input as is
      * inputField - The field from the input data you would like to map
  * FirstAvailableMapping - Gets first available field from input data
      * inputFields - Array of input fields you want to check (in order)
  * TemplateMapping - Can concatenate data in a template format
      * template - Template to use for mapping, wrap field names in $[] (e.g. "Title is $[Title]")
  * ScriptMapping - Can use Powershell to map data*
      * script - Powershell script that returns data, $inputValues dictionary available

* Transformations
  * SplitStringTransformation
      * delimiter - The delimiter you'd like to split on
  * SearchStringTransformation - Searches for a pattern in the string
      * pattern - The regular expression you want to search format
      * group - The capture group you're interested in (defaults to 0)
  * FindReplaceStringTransformation - Replaces string / pattern with another string
      * find - String / pattern
      * replace - Replace String
      * regex - Boolean indicating whether to use a regular expression search
  * ConvertToDateTransformation - Converts input data to a DateTime object
      * format - Date input Transformations
      * ignoreInvalid - if false, throws exception on invalid data
  * ConvertFromDateTransformation - Converts input date object to a string
      * format - Desired data output format
  * HashStringTransformation - Hashes the input string
      * algorithm (md5 / sha256)
  * ReadFileTransformation - Reads file at the provided file path into a string
      * encoding - File encoding (utf8 / ansii / utf16)
  * HashFileTransformation
      * algorithm (md5 / sha256)
  * ScriptTransformation - Can use Powershell to transform data*
      * script - Powershell script that returns data, $inputValue object available


## TODO
- Increase test coverage of C# classes
- Implement basic Pester tests for Cmdlets
- Finalise remapping API and fully implement in Powershell
