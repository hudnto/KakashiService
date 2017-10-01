using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
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

        public static void WriteFileFromStream(Stream stream, string path)
        {
            var file = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            stream.Position = 0;
            stream.CopyTo(file);
            file.Close();
        }


        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

    }
}
