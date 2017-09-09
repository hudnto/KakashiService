using KakashiService.Core.Entities;
using KakashiService.Core.Modules.Create;

namespace KakashiService.Core.Services
{
    public class CreateService
    {
        public void Execute(ServiceObject service)
        {
            CreateFile.SetConfig(service.Path, service.Name, service.Namespace, service.LogPath);
            CreateFile.FileIService(service.Functions);
            CreateFile.FileService(service.Functions, service.OriginServiceName);
            CreateFile.FileServiceSVC();
            CreateFile.FileProj(service.OriginServiceName, service.ObjectTypes);
            CreateFile.WebConfig();
            CreateFile.Package();
            CreateFile.Solution();
            CreateFile.CreateProxyClassFromStream(service);
        }
    }
}