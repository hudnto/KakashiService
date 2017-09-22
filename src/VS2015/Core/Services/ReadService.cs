

using System.IO;
using KakashiService.Core.Entities;
using KakashiService.Core.Modules.Read;

namespace KakashiService.Core.Services
{
    public class ReadService
    {
        public void Execute(ServiceObject serviceObject)
        {
            var readServiceInfo = new ReadServiceInfo();

            var serviceDescription = readServiceInfo.GetServiceDescriptionFromObject(serviceObject);

            readServiceInfo.ExtractAllFromService(serviceDescription, serviceObject);

            serviceObject.OriginServiceName = string.IsNullOrEmpty(serviceDescription.Name) ? serviceDescription.PortTypes[0].Name : serviceDescription.Name;
        }
    }
}
