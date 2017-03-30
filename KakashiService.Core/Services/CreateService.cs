using KakashiService.Core.Entities;
using KakashiService.Core.Modules.Create;

namespace KakashiService.Core.Services
{
    public class CreateService
    {
        public void Execute(ServiceObject service)
        {
            CreateFile.SetConfig(service.Path, service.Name, service.Namespace);
            CreateFile.FileIService(service.Functions);
            CreateFile.FileService(service.Functions, service.OriginServiceName);
            CreateFile.FileServiceSVC();
            CreateFile.FileDataContract(service.ObjectTypes);
            CreateFile.FileProj(service.OriginServiceName, service.ObjectTypes);
            CreateFile.WebConfig();
        }
    }
}