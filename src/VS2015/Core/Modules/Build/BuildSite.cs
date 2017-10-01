using Microsoft.Web.Administration;
using System;
using System.IO;

namespace KakashiService.Core.Modules.Build
{
    public static class BuildSite
    {
        public static void Create(string siteName, int port, string path)
        {
            path = path + siteName;
            CreatePath(path);

            AddSite(siteName, (mgr, site) =>
            {
                site.SetPhysicalPath(path);
                site.BindToPort(port);
                site.ServerAutoStart = true;
            });
        }

        private static void CreatePath(string path)
        {
            // test path and create if doesnt exists
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        /// <summary>
        /// https://visualstudiomagazine.com/Articles/2014/06/01/Automating-IIS-7.aspx?Page=1
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="siteConfigurator"></param>
        private static void AddSite(string siteName, Action<ServerManager, Site> siteConfigurator)
        {
            using (var sm = new ServerManager())
            {
                var invalidChars = SiteCollection.InvalidSiteNameCharacters();
                if (siteName.IndexOfAny(invalidChars) > -1)
                {
                    throw new Exception(String.Format("Invalid Site Name: {0}", siteName));
                }

                var site = sm.Sites.Add(siteName, "", 0);
                siteConfigurator(sm, site);

                sm.CommitChanges();
            }
        }
    }
}
