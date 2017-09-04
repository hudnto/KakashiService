using KakashiService.Core.Entities;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace KakashiService.Core.Modules.Build
{
    public static class BuildTemplate
    {
        // using SvcUtil
        public static void CreateProxyClass(ServiceObject service)
        {
            var command = Util.GetTemplate("PowerShellScript.svcutil.ps1");

            command = command.Replace("@svcutilPath", service.SvcUtilPath);
            command = command.Replace("@projectPath", service.Path);

            if (service.FileStream != null)
            {
                var filePath = service.Path + "//proxy.wsdl";
                var memory = new MemoryStream();
                service.FileStream.CopyTo(memory);
                FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                memory.WriteTo(file);
                file.Close();
                memory.Close();
                service.Url = filePath;
            }

            command = command.Replace("@url", service.Url);

            command = command.Replace("@originService", service.OriginServiceName);
            command = command.Replace("@namespace", service.Namespace);


            using (PowerShell shell = PowerShell.Create())
            {
                shell.Commands.AddScript(command);

                var results = shell.Invoke();
                var errors = shell.Streams.Error.ToList();
            }
        }

        public static void Restore(string nugetPath, string projectPath)
        {
            var command = Util.GetTemplate("PowerShellScript.restore.ps1");

            var solutionPath = projectPath.Replace(".csproj", ".sln");

            command = command.Replace("@solutionPath", solutionPath);
            command = command.Replace("@nugetPath", nugetPath);

            using (PowerShell shell = PowerShell.Create())
            {
                shell.Commands.AddScript(command);

                var results = shell.Invoke();
                var errors = shell.Streams.Error.ToList();
            }
        }

        public static void Build(string projectPath, string msbuildPath)
        {
            var command = Util.GetTemplate("PowerShellScript.build.ps1");

            command = command.Replace("@msbuildPath", msbuildPath);
            command = command.Replace("@projectPath", projectPath);

            using (PowerShell shell = PowerShell.Create())
            {
                shell.Commands.AddScript(command);

                var results = shell.Invoke();
                var errors = shell.Streams.Error.ToList();
            }
        }

        public static void MoveBin(string source, string destin)
        {
            var command = Util.GetTemplate("PowerShellScript.moveBin.ps1");

            command = command.Replace("{path}", source);
            command = command.Replace("{isspath}", destin);

            using (PowerShell shell = PowerShell.Create())
            {
                shell.Commands.AddScript(command);

                var results = shell.Invoke();
                var errors = shell.Streams.Error.ToList();
            }
        }
    }
}
