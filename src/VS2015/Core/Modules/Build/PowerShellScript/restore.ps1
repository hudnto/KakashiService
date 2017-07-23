$solution = "@solutionPath"
$path = "C:\inetpub\Kakashi\Bin\bin" #Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
$nuget =  $path + "@nugetPath"

& $nuget restore $solution 