################################################################################
# Author     : Alex Morris (alex@aemcreative.co.uk)
# Company    : Financial Conduct Authority
################################################################################
#  This work is licensed under a Creative Commons
#  Attribution-NonCommercial-NoDerivatives 4.0 International License.
#  http://creativecommons.org/licenses/by-nc-nd/4.0/
################################################################################

Get-ChildItem -Path $psScriptRoot + '\functions' `
    | ? { $_ -match '^func_.+$' } `
    | % {
        . (Join-Path -Path $psScriptRoot -ChildPath $_)
    }
