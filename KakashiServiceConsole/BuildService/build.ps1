$project = "@projectPath"
$msbuild = "@msbuildPath"

& $msbuild $project "/p:Configuration=Debug"

