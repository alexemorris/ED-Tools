# ED-Tools
Some incredibly useful parsers for importing, remapping and exporting various common datatypes in the eDiscovery Sector (Concordance, Opticon, IDX). Includes various C# Powershell Cmdlets. 

https://www.sec.gov/divisions/enforce/datadeliverystandards.pdf


## Parsers
* Concordance Data
1. Import-Concordance 
2. Export-Concordance 
3. ConvertFrom-Concordance
4. ConvertTo-Concordance

* Opticon Data
1. Import-OPT 
2. Export-OPT 
3. ConvertFrom-OPT
4. ConvertTo-OPT

* IDX Data*
1. Import-IDX 
2. ConvertFrom-IDX 

* IDX data is very specific to Autonomy / IDOL eDiscovery platforms. May move these features to separate library.


## Remapping
*Still in development!*
.NET classes and corresponding Cmdlets that can remap input data columns, parse dates, delimited fields and email data. 

## TODO
- Increase test coverage of C# classes
- Implement basic Pester tests for Cmdlets
- Finalise remapping API and fully implement in Powershell


