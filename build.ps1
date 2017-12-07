param($inputdir, $outputdir, $release) 

Write-Host $inputdir 
Write-Host $outputdir
Write-Host ($release -ne $true)


rm -Recurse $outputdir
mkdir  $outputdir
