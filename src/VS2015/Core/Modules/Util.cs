﻿using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using KakashiService.Core.Modules.Create;

namespace KakashiService.Core.Modules
{
    public static class Util
    {
        public static String NormalizeVariable(String variable)
        {
            switch (variable)
            {
                case "dateTime":
                case "datetime": return "DateTime";
                case "boolean": return "bool";
            }
            return variable;
        }

        public static string GetTemplate(string templatePath)
        {
            // Get Resource file 
            var fileName = templatePath;
            var assembly = Assembly.GetExecutingAssembly();
            var allResources = assembly.GetManifestResourceNames();
            var resourceName = allResources.First(a => a.Contains(fileName));

            var value = String.Empty;
            // read model in txt
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                value = reader.ReadToEnd();
            }
            return value;
        }

        public static void CreateFile(string projectPath, string temp)
        {
            FileInfo file = new FileInfo(projectPath);
            DirectoryInfo di = new DirectoryInfo(file.DirectoryName);
            if (!di.Exists)
            {
                di.Create();
            }

            if (!file.Exists)
            {
                using (var stream = file.CreateText())
                {
                    stream.WriteLine(temp);
                }
            }
        }
    }
}