using System;
using System.Linq;
using Microsoft.Web.Administration;

namespace KakashiService.Core.Modules.Build
{
    static class Extensions
    {
        public static void SetPhysicalPath(this Site site, string path)
        {
            site.Applications.First().VirtualDirectories.First().PhysicalPath = path;
        }

        public static void BindToPort(this Site site, int port)
        {
            site.Bindings.First().BindingInformation = String.Format("*:{0}:", port);
        }
    }
}