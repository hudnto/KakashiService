

using KakashiService.Core.Entities;
using KakashiService.Core.Modules.Read;

namespace KakashiService.Core.Services
{
    public class ReadService
    {
        public void Execute(ServiceObject serviceObject)
        {
            var readServiceInfo = new ReadServiceInfo();

            var serviceDescription = readServiceInfo.GetServiceDescriptionFromSVC(serviceObject.Url);

            var schemaSet = readServiceInfo.GetAllSchema(serviceDescription);

            serviceObject.Functions = readServiceInfo.ExtractItemXml(schemaSet);

            serviceObject.ObjectTypes = readServiceInfo.GetObjectTypes();

            serviceObject.OriginServiceName = serviceDescription.Name;
        }
    }
}
