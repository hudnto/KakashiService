

using KakashiService.Core.Entities;
using KakashiService.Core.Modules.Read;

namespace KakashiService.Core.Services
{
    public class ReadService
    {
        public void Execute(ServiceObject serviceObject)
        {
            var serviceDescription = Util.GetServiceDescriptionFromSVC(serviceObject.Url);

            var schemaSet = Util.GetAllSchema(serviceDescription);

            serviceObject.Functions = Util.ExtractItemXml(schemaSet);

            serviceObject.OriginServiceName = serviceDescription.Name;
        }
    }
}
