function New-ConcordanceMapping {
  <#
  .SYNOPSIS
  Run this command to generate a new structured load mapping object
  .DESCRIPTION
  Can be provided with a JSON configuration (see documentation for schema), or ask questions based on a structured load file
  .EXAMPLE
  $mapping = New-StructuredLoadMapping -Config "mapping.json"
  .EXAMPLE
  $mapping = New-StructuredLoadMapping -Data "concordance.dat"
  .PARAMETER Config
  The path to a structured load file
  .PARAMETER Data
  The path to an existing structured load
  #>
  [CmdletBinding()]
  param
  (
    [parameter(Position=0,
   Mandatory=$true,
   ParameterSetName="ByConfig")]
   [string]$Config,

   [parameter(Position=0,
   Mandatory=$true,
   ParameterSetName="ByData")]
   [string]$Data

  )


  process {

      function parseJson($file) {
          $mappings = Get-Content "\\Mac\Home\Desktop\RECC026-142446\mapping.json" | ConvertFrom-Json

          $tuples = @()

          $mapper = $mappings | % {
              $mapping = $_
              $name = $mapping.name

              switch($mapping.mapping.merge) {
                  "FavourInput" {
                      $merge = [edtools.Remap.MergeType]::FAVOUR_INPUT
                  }
                  "FavourOutput" {
                      $merge = [edtools.Remap.MergeType]::FAVOUR_OUTPUT
                  }
                  "Output" {
                      $merge = [edtools.Remap.MergeType]::OUTPUT
                  }
                  "Input" {
                      $merge = [edtools.Remap.MergeType]::INPUT
                  }
                  default {
                      $merge = [edtools.Remap.MergeType]::FAVOUR_INPUT
                  }
              }


              switch($mapping.mapping.type) {
                  "DirectMapping" {
                      $mapper = New-Object -TypeName "edtools.Remap.Mapping.DirectMapping" -ArgumentList $mapping.mapping.args.fieldname
                  }
                  "FirstAvailableMapping" {
                      $mapper = New-Object -TypeName "edtools.Remap.Mapping.FirstAvailableMapping" -ArgumentList (,$mapping.mapping.args.fieldnames)
                  }
                  "ScriptMapping" {
                      $mapper = New-Object -TypeName "edtools.Remap.Mapping.ScriptMapping" -ArgumentList $mapping.mapping.args.script
                  }
                  "TemplateMapping" {
                      $mapper = New-Object -TypeName "edtools.Remap.Mapping.TemplateMapping" -ArgumentList $mapping.mapping.args.template
                  }
                  default {
                      Write-Error "Type $($mapping.mapping.type) invalid"
                  }
              }

              $transformers= @()
              $transformer = $null

              foreach ($transformation in $mapping.transformations) {
                  switch($transformation.type) {
                      "HashFileTransformation" {
                          $ignoreMissing = $transformation.args.ignoreMissing -eq $true
                          $transformer = New-Object -TypeName "edtools.Remap.Transformation.HashFileTransformation" -ArgumentList $ignoreMissing
                      }
                      "HashStringTransformation" {
                          $transformer = New-Object -TypeName "edtools.Remap.Transformation.HashStringTransformation" -ArgumentList $transformation.args.algorithm
                      }
                      "ScriptTransformation" {
                          $transformer = New-Object -TypeName "edtools.Remap.Transformation.ScriptTransformation" -ArgumentList $transformation.args.script
                      }
                      "FindReplaceTransformation" {
                          $regex = $transformation.args.regex -eq $true
                          $transformer = New-Object -TypeName "edtools.Remap.Transformation.FindReplaceTransformation" -ArgumentList $transformation.args.find, $transformation.args.replace,$regex
                      }
                      "ReadFileTransformation" {
                          $encoding = $transformation.args.encoding
                          $ignoreMissing = $transformation.args.ignoreMissing -eq $true
                          if ($encoding) {
                              $transformer = New-Object -TypeName "edtools.Remap.Transformation.ReadFileTransformation" -ArgumentList $encoding,$ignoreMissing
                          } else {
                              $transformer = New-Object -TypeName "edtools.Remap.Transformation.ReadFileTransformation" -ArgumentList $ignoreMissing
                          }
                      }
                      "FileInfoTransformation" {
                          $root = $transformation.args.root
                          $checkExists = $transformation.args.checkExists -eq $true
                          $transformer = New-Object -TypeName "edtools.Remap.Transformation.FileInfoTransformation" -ArgumentList $checkExists,$root
                      }
                      "ConvertToDateTransformation" {
                          $ignoreinvalid = $transformation.args.ignoreInvalid -eq $true
                          $transformer = New-Object -TypeName "edtools.Remap.Transformation.ConvertToDateTransformation" -ArgumentList $transformation.args.format,$ignoreinvalid
                      }
                      "ConvertToStringTransformation" {
                          $transformer = New-Object -TypeName "edtools.Remap.Transformation.ConvertToStringTransformation"
                      }
                      "SplitTransformation" {
                          $transformer = New-Object -TypeName "edtools.Remap.Transformation.ConvertToStringTransformation"
                      }
                      "SearchStringTransformation" {
                          $group = 0
                          if ($transformation.args.group) {
                              $group = $transformation.args.group
                          }
                          $transformer = New-Object -TypeName "edtools.Remap.Transformation.SearchStringTransformation" -ArgumentList $transformation.args.pattern,$group
                      }
                      "ConvertFromDateTransformation" {
                          $ignoreinvalid = $transformation.args.ignoreInvalid -eq $true
                          $transformer = New-Object -TypeName "edtools.Remap.Transformation.ConvertFromDateTransformation" -ArgumentList $transformation.args.format,$ignoreinvalid
                      }
                      "MailAddressTransformation" {
                          $ignoreinvalid = $transformation.args.ignoreInvalid -eq $true
                          $transformer = New-Object -TypeName "edtools.Remap.Transformation.MailAddressTransformation" -ArgumentList $ignoreinvalid
                      }
                      "SplitStringTransformation" {
                          $ignoreMissing = $transformation.args.ignoreMissing -eq $true
                          $transformer = New-Object -TypeName "edtools.Remap.Transformation.SplitStringTransformation" -ArgumentList $transformation.args.separator,$ignoreMissing
                      }
                      default {
                          Write-Error "Transformation $($transformation.type) invalid"
                      }
                  }
                  if ($transformer) {
                      $transformers += $transformer
                  }

              }

              $fullMapping = New-Object -TypeName "edtools.Remap.SingleMapping" -ArgumentList $mapper,$transformers,$merge
              return [System.Tuple]::Create($name, $fullMapping)
          }

          return New-Object -TypeName "edtools.Remap.FullMapping" -ArgumentList (,$mapper)
      }

      function newMerge() {
          $validMerges = "FavourInput", "FavourOutput", "Output", "Input"
          Write-Host "    Please choose a input merge type ($($validMerges -join ", "))"
          while (!$merge) {
              $inputMerge = Read-Host -Prompt "    Choose a merge type"
              if ($inputMerge -in $validMerges) {
                  $merge = $inputMerge
                  Write-Host "      Using $merge for merging" -ForegroundColor Green
              } elseif (!$inputMerge) {
                  Write-Host "      No merge provided, using default merge (Favour Input)" -ForegroundColor Green
                  $merge = "FavourInput"
              } else {
                  Write-Host "      Merge type $inputMerge not recognized" -ForegroundColor Red
              }
          }
          Write-Host ""

          switch($merge) {
              "FavourInput" {
                  $merge = [edtools.Remap.MergeType]::FAVOUR_INPUT
              }
              "FavourOutput" {
                  $merge = [edtools.Remap.MergeType]::FAVOUR_OUTPUT
              }
              "Output" {
                  $merge = [edtools.Remap.MergeType]::OUTPUT
              }
              "Input" {
                  $merge = [edtools.Remap.MergeType]::INPUT
              }
              default {
                  $merge = [edtools.Remap.MergeType]::FAVOUR_INPUT
              }
          }

          return $merge
      }

      function newMapping($headingHash, $outputHash) {

          $validTypes = "Direct", "Template", "FirstAvailable", "Script"
          while (!$type) {
              $typeInput = Read-Host -Prompt "    Please choose a mapping type ($($validTypes -join ", "))"
              if ($typeInput -in $validTypes) {$type = $typeInput}
          }

          Write-Host "      Using $type mapping!`n" -ForegroundColor Green

          switch ($type) {
              "Direct" {
                  Write-Host "    Please choose the column you'd like to map"
                  while (!$direct) {
                      $directInput = Read-Host -Prompt "     Type column input name"
                      if ($headingHash.Contains($directInput)) {
                          $direct = $directInput
                          Write-Host "      Using $direct for direct mapping!" -ForegroundColor Green
                      } else { Write-Host "       Column $directInput does not exist!" -ForegroundColor Red }
                  }
                  $mapper = New-Object -TypeName "edtools.Remap.Mapping.DirectMapping" -ArgumentList $direct
              }
              "Template" {
                  Write-Host "    Please define your template in the following format - '$[Col1] $[Col2]'"
                  while (!$template) {
                      $templateInput = Read-Host -Prompt "    Define a valid template"
                      $templateColumns = $templateInput | Select-String -Pattern "\$\[([^\]]+)\]" -AllMatches
                      $valid = $true
                      foreach ($match in $templateColumns.Matches) {
                          if (!$headingHash.Contains($match.Groups[1].Value)) {
                              $valid = $false
                              Write-Host "      Column $($match.Groups[1].Value) does not exist!" -ForegroundColor Red
                          }
                      }
                      if ($valid) { $template = $templateInput; Write-Host "      Using $template for template mapping!" -ForegroundColor Green}
                  }
                  $mapper = New-Object -TypeName "edtools.Remap.Mapping.TemplateMapping" -ArgumentList $template
              }
              "Script" {
                  Write-Host "    Please define your script (input values can be obtained using the `$inputValues variable), return your output"
                  while (!$script) {
                      $scriptInput = Read-Host -Prompt "      Define a script"
                      if ($scriptInput -match "\`$inputValues") {
                          $script = $scriptInput
                      } else {
                          Write-Host "        `$inputValues not accessed in script!" -ForegroundColor Yellow
                          $warning = Read-Host "        Are you sure you want to proceed? Y/N"
                          if ($warning -eq "Y") {
                              $script = $scriptInput
                          }
                      }
                  }
                  Write-Host "      Using script '$script'!" -ForegroundColor Green
                  $mapper = New-Object -TypeName "edtools.Remap.Mapping.ScriptMapping" -ArgumentList $script
              }
              "FirstAvailable" {
                  Write-Host "    Define your input columns"
                  $inputColumns = @()
                  $end = $false
                  while (!$end) {
                      $added = $false
                      while(!$added) {
                          $colInput = Read-Host -Prompt "      Choose a column name (leave blank to finish)"
                          if ($headingHash.Contains($colInput)) {
                              $inputColumns += $colInput
                              $added = $true
                          } elseif(!$colInput) {
                              $end = $true
                              $added = $true
                          } else {
                              Write-Host "      Column $colInput does not exist!" -ForegroundColor Red
                          }
                      }
                      if ($inputColumns.Count -eq 0) {
                          $end = $false
                      } elseif($end) {
                          Write-Host "      Using $($inputColumns -join ", ") as input!" -ForegroundColor Green
                      }

                  }
                  $mapper = New-Object -TypeName "edtools.Remap.Mapping.FirstAvailableMapping" -ArgumentList (,$inputColumns)
              }
          }
          Write-Host ""
          return $mapper
      }
      
     switch ($psCmdlet.ParameterSetName) {

        "ByConfig" {
            return (parseJson $Config)
        }
        "ByData" {
            $header = Import-ConcordanceHeader $Data
            $columns = "DocID","AttachID","BatesBegin","BatesEnd","AttachBegin","AttachEnd","FileName","FROM","TO","CC","BCC","NATIVE_LINK","TEXT_LINK"
            $dates = "Created Date","Sent Date","Recieved Date","Modified Date","Accessed Date","Document Date"

            $headingHash = @{}
            $firstValuesHash = @{}

            foreach ($heading in $header) {
                $headingHash[$heading] = $true
                $firstValues = (Import-Concordance $Data -WarningAction SilentlyContinue| % { $_.$heading } | ? { $_ } | Select -First 3) -join ", "
                if ($firstValues) {
                    $firstValuesHash[$heading] = $firstValues
                    Write-Host "Column: $heading"
                    Write-Host "    First 3 Values: $firstValues"
                    Write-Host ""
                }
            }

            foreach ($heading in $header) {
                if (-not $firstValuesHash.Contains($heading)) {
                    Write-Host "Empty Column: $heading"
                    Write-Host ""
                }
            }

            $mapper = @()

            foreach ($column in $columns) {
                Write-Host "Need to map column $column"
                $merge = newMerge
                $mapping = newMapping $headingHash
                $fullMapping = New-Object -TypeName "edtools.Remap.SingleMapping" -ArgumentList $mapping,@(),$merge
                $mapper += [System.Tuple]::Create($column, $fullMapping)
            }

            foreach ($column in $dates) {
                Write-Host "Mapping column: $column" -ForegroundColor Green
                $merge = newMerge
                $mapping = newMapping $headingHash
                $fullMapping = New-Object -TypeName "edtools.Remap.SingleMapping" -ArgumentList $mapping,@(),$merge
                $mapper += [System.Tuple]::Create($column, $fullMapping)
            }

            return New-Object -TypeName "edtools.Remap.FullMapping" -ArgumentList (,$mapper)
        }
     }

  }
}
