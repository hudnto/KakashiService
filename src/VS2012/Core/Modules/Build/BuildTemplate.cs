﻿using KakashiService.Core.Entities;
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
            // Get Resource file 
            var fileName = "PowerShellScript.svcutil.ps1";
            var assembly = Assembly.GetExecutingAssembly();
            var allResources = assembly.GetManifestResourceNames();
            var resourceName = allResources.First(a => a.Contains(fileName));

            String command = String.Empty;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                command = reader.ReadToEnd();
            }

            command = command.Replace("@svcutilPath", service.SvcUtilPath);
            command = command.Replace("@projectPath", service.Path);
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
            // Get Resource file 
            var fileName = "PowerShellScript.restore.ps1";
            var assembly = Assembly.GetExecutingAssembly();
            var allResources = assembly.GetManifestResourceNames();
            var resourceName = allResources.First(a => a.Contains(fileName));

            var solutionPath = projectPath.Replace(".csproj", ".sln");

            String command = String.Empty;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                command = reader.ReadToEnd();
            }

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
            // Get Resource file 
            var fileName = "PowerShellScript.build.ps1";
            var assembly = Assembly.GetExecutingAssembly();
            var allResources = assembly.GetManifestResourceNames();
            var resourceName = allResources.First(a => a.Contains(fileName));

            String command = String.Empty;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
             using (StreamReader reader = new StreamReader(stream))
            {
                command = reader.ReadToEnd();
            }

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
            // Get Resource file 
            var fileName = "PowerShellScript.moveBin.ps1";
            var assembly = Assembly.GetExecutingAssembly();
            var allResources = assembly.GetManifestResourceNames();
            var resourceName = allResources.First(a => a.Contains(fileName));

            String command = String.Empty;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                command = reader.ReadToEnd();
            }

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
