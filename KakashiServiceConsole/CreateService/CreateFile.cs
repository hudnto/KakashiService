
using KakashiServiceConsole.ReadService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace KakashiServiceConsole.CreateService
{
    public class CreateFile
    {
        public static String _path;
        public static String _serviceName;
        public static String _namespaceValue;
        public static void SetConfig(string path, string serviceName, string namespaceValue)
        {
            _path = path;
            _serviceName = serviceName;
            _namespaceValue = namespaceValue;
        }
        public static void FileIService(List<Functions> functions)
        {
            // get file from the resource
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "KakashiServiceConsole.CreateService.Interface.txt";

            var value = String.Empty;
            // read model in txt
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                value = reader.ReadToEnd();
            }

            // replace all tags, except body
            value = value.Replace("{namespace}", _namespaceValue);
            value = value.Replace("{serviceName}", _serviceName);

            string functionValue = String.Empty;
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();
            // replace body with functions
            foreach (var function in functions)
            {
                var parametersValue = String.Empty;
                int index = 0;
                for (int i = 0; i < function.Parametros.Count; i++)
                {
                    var type = function.Parametros[i].Type.GetDescription();
                    var comma = function.Parametros.Count == i + 1 ? String.Empty : ", ";
                    parametersValue = parametersValue + String.Format("{0} {1}{2}", type, alpha[i], comma);
                    index++;
                }

                functionValue = functionValue + String.Format("[OperationContract]\n\t\t{0} {1} ({2});\n\t\t", function.ReturnType.GetDescription(), function.Name, parametersValue);
            }

            value = value.Replace("{body}", functionValue);

            // create new file in the path with string value

            FileInfo file = new FileInfo(_path + "/I" + _serviceName + ".cs");
            DirectoryInfo di = new DirectoryInfo(file.DirectoryName);
            if (!di.Exists)
            {
                di.Create();
            }

            if (!file.Exists)
            {
                using (var stream = file.CreateText())
                {
                    stream.WriteLine(value);
                }
            }
        }

        public static void FileService(List<Functions> functions, String originService)
        {
            // get file from the resource
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "KakashiServiceConsole.CreateService.Service.txt";

            var value = String.Empty;
            // read model in txt
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                value = reader.ReadToEnd();
            }

            // replace all tags, except body
            value = value.Replace("{namespace}", _namespaceValue);
            value = value.Replace("{serviceName}", _serviceName);
            value = value.Replace("{originService}", originService);

            string functionValue = String.Empty;
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();

            foreach (var function in functions)
            {
                string arguments = String.Empty;
                var parametersValue = String.Empty;
                int index = 0;
                for (int i = 0; i < function.Parametros.Count; i++)
                {
                    var type = function.Parametros[i].Type.GetDescription();
                    var comma = function.Parametros.Count == i + 1 ? String.Empty : ", ";
                    parametersValue = parametersValue + String.Format("{0} {1}{2}", type, alpha[i], comma);
                    arguments = arguments + alpha[i] + " " + comma;
                    index++;
                }

                functionValue = functionValue + String.Format("public {0} {1} ({2})", function.ReturnType.GetDescription(), function.Name, parametersValue);
                functionValue = functionValue + "{\n" + String.Format("\treturn _client.{0}({1});", function.Name, arguments) + "\n}\n\t\t";
            }

            value = value.Replace("{body}", functionValue);

            FileInfo file = new FileInfo(_path + "/" + _serviceName + ".svc.cs");
            DirectoryInfo di = new DirectoryInfo(file.DirectoryName);
            if (!di.Exists)
            {
                di.Create();
            }

            if (!file.Exists)
            {
                using (var stream = file.CreateText())
                {
                    stream.WriteLine(value);
                }
            }
        }

        public static void FileServiceSVC()
        {
            // get file from the resource
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "KakashiServiceConsole.CreateService.ServiceSVC.txt";

            var value = String.Empty;
            // read model in txt
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                value = reader.ReadToEnd();
            }

            value = value.Replace("{namespace}", _namespaceValue);
            value = value.Replace("{serviceName}", _serviceName);

            // create new file in the path with string value

            FileInfo file = new FileInfo(_path + "/" + _serviceName + ".svc");
            DirectoryInfo di = new DirectoryInfo(file.DirectoryName);
            if (!di.Exists)
            {
                di.Create();
            }

            if (!file.Exists)
            {
                using (var stream = file.CreateText())
                {
                    stream.WriteLine(value);
                }
            }
        }

        public static void FileProj(string originService)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "KakashiServiceConsole.CreateService.Proj.txt";

            var value = String.Empty;
            // read model in txt
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                value = reader.ReadToEnd();
            }

            value = value.Replace("{namespace}", _namespaceValue);
            value = value.Replace("{serviceName}", _serviceName);
            value = value.Replace("{originService}", originService);


            FileInfo file = new FileInfo(_path + "/" + _serviceName + ".csproj");
            DirectoryInfo di = new DirectoryInfo(file.DirectoryName);
            if (!di.Exists)
            {
                di.Create();
            }

            if (!file.Exists)
            {
                using (var stream = file.CreateText())
                {
                    stream.WriteLine(value);
                }
            }
        }

        public static void WebConfig()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "KakashiServiceConsole.CreateService.Webconfig.txt";

            var value = String.Empty;
            // read model in txt
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                value = reader.ReadToEnd();
            }

            FileInfo file = new FileInfo(_path + "/Web.config");
            DirectoryInfo di = new DirectoryInfo(file.DirectoryName);
            if (!di.Exists)
            {
                di.Create();
            }

            if (!file.Exists)
            {
                using (var stream = file.CreateText())
                {
                    stream.WriteLine(value);
                }
            }
        }
    }
}
