################################################################################
# Author     : Antony Onipko (antony.onipko@ravn.co.uk)
# Company    : RAVN Systems Ltd.
# Copyright  : (c) 2016 RAVN Systems Ltd. All rights reserved.
################################################################################
#  This work is licensed under a Creative Commons
#  Attribution-NonCommercial-NoDerivatives 4.0 International License.
#  http://creativecommons.org/licenses/by-nc-nd/4.0/
#  Permissions beyond the scope of this license may be available at
#  http://www.ravn.co.uk.
################################################################################

Get-ChildItem -Path $psScriptRoot `
    | ? { $_ -match '^Func_.+$' } `
    | % {
        . (Join-Path -Path $psScriptRoot -ChildPath $_)
    }
