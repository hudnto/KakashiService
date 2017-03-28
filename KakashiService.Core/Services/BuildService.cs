using KakashiService.Core.Entities;
using KakashiService.Core.Modules.Build;
using System;

namespace KakashiService.Core.Services
{
    public class BuildService
    {
        public void Execute(ServiceObject service)
        {
            BuildTemplate.CreateProxyClass(service);

            var projectPath = String.Format("{0}/{1}.csproj", service.Path, service.Name);

            BuildTemplate.Build(projectPath, service.MsBuildPath);

            BuildSite.Create(service.Name, service.Port, service.IISPath);

            var binWebSitePath = String.Format("{0}/{1}", service.IISPath, service.Name);

            BuildTemplate.MoveBin(service.Path, binWebSitePath);
        }
    }
}