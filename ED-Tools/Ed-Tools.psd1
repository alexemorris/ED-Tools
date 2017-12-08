################################################################################
#  This work is licensed under a Creative Commons
#  Attribution-NonCommercial-NoDerivatives 4.0 International License.
#  http://creativecommons.org/licenses/by-nc-nd/4.0/
#  Permissions beyond the scope of this license may be available at
#  http://www.ravn.co.uk.
################################################################################

@{

# Script module or binary module file associated with this manifest.
# ModuleToProcess = ''

# Version number of this module.
# ModuleVersion = '{{ MODULE_VERSION }}'

ModuleVersion = '1.0'

# ID used to uniquely identify this module
GUID = '02c6cf60-db5b-11e7-9996-37883524dd6d'

# Author of this module
Author = 'Alex Morris'

# Company or vendor of this module
CompanyName = 'Financial Conduct Authority'

# Copyright statement for this module
# Copyright = ''

# Description of the functionality provided by this module
Description = 'A collection of eDiscovery tools for importing and remapping data.'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '4.0'

# Name of the Windows PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of Microsoft .NET Framework required by this module
# DotNetFrameworkVersion = ''

# Minimum version of the common language runtime (CLR) required by this module
# CLRVersion = ''

# Processor architecture (None, X86, Amd64) required by this module
# ProcessorArchitecture = ''

# Modules that must be imported into the global environment prior to importing this module
# RequiredModules = @(
#     ''
# )

# Assemblies that must be loaded prior to importing this module

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
# ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
# TypesToProcess = @()

# Format files (.ps1xml) to be loaded when importing this module
# FormatsToProcess = @()

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
NestedModules = @(
    'Ed-Tools.psm1'
    'dll\edtools.dll'
)

# Functions to export from this module
# FunctionsToExport = '*'

# Cmdlets to export from this module
# CmdletsToExport = '*'

# Variables to export from this module
# VariablesToExport = '*'

# Aliases to export from this module
# AliasesToExport = '*'

# List of all modules packaged with this module
# ModuleList = @()

# List of all files packaged with this module
# FileList = @()

# Private data to pass to the module specified in RootModule/ModuleToProcess
# PrivateData = ''

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''

}
