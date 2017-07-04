$solution = "@solutionPath"
$project = "@projectPath"
$nuget = "@nugetPath"
$msbuild = "@msbuildPath"

& $nuget restore $solution
& $msbuild $project "/p:Configuration=Debug;VisualStudioVersion=14.0"

