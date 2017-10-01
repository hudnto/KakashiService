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

            readServiceInfo.GetInfoFromService(serviceObject);
        }
    }
}
