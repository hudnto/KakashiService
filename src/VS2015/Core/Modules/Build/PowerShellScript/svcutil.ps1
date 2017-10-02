$destin = "@projectPath"
$svcutil = "@svcutilPath"
$namespace = "@namespace"

& $svcutil @url /mergeConfig /language:cs /d:$destin /o:@originService /config:"$destin/Web.config" #/n:*,$namespace

