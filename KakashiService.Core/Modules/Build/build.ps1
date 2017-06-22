$project = "@projectPath"
$msbuild = "@msbuildPath"

& $msbuild $project "/p:Configuration=Debug;VisualStudioVersion=14.0"

