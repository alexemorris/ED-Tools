# ED-Tools
Some incredibly useful parsers for importing, remapping and exporting various common datatypes in the eDiscovery workflows (Concordance, Opticon, IDX). Includes various C# Powershell Cmdlets. 

https://www.sec.gov/divisions/enforce/datadeliverystandards.pdf


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

* IDX Data*
  * Import-IDX 
  * ConvertFrom-IDX 

*IDX data is very specific to Autonomy / IDOL eDiscovery platforms. May move these features to separate library.


## Remapping
*Still in development!*
.NET classes and corresponding Cmdlets that can remap input data columns, parse dates, delimited fields and email data. 

## TODO
- Increase test coverage of C# classes
- Implement basic Pester tests for Cmdlets
- Finalise remapping API and fully implement in Powershell


