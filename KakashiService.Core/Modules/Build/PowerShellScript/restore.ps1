$solution = "@solutionPath"
$path = Split-Path -parent $MyInvocation.MyCommand.Definition
$nuget =  $path + "@nugetPath"

Write-Host $path
Write-Host $nuget

& $nuget restore $solution -Verbosity

