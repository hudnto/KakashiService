$destin = "@projectPath"
$svcutil = "@svcutilPath"

& $svcutil @url /mergeConfig /language:cs /d:$destin /o:@originService /config:"$destin/Web.config" 

