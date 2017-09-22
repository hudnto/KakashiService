using KakashiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using KakashiService.Core.Modules.Read;

namespace KakashiService.Core.Modules.Create
{
    public class CreateFile
    {
        public static String _path;
        public static String _serviceName;
        public static String _namespaceValue;
        public static String _logPath;

        public static void SetConfig(string path, string serviceName, string namespaceValue, string logPath)
        {
            _path = path;
            _serviceName = serviceName;
            _namespaceValue = namespaceValue;
            _logPath = logPath;
            DirectoryInfo di = new DirectoryInfo(_path);
            if (!di.Exists)
            {
                di.Create();
            }
            else
            {
                di.Delete(true);
                di.Create();
            }
        }

        public static void FileIService(List<Functions> functions)
        {
            var value = Util.GetTemplate("TemplatesFile.Interface.txt");

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
                for (int i = 0; i < function.Parameters.Count; i++)
                {
                    var type = function.Parameters[i].Type;
                    var comma = function.Parameters.Count == i + 1 ? String.Empty : ", ";
                    parametersValue = parametersValue + String.Format("{0} {1}{2}", type, alpha[i], comma);
                    index++;
                }

                functionValue = functionValue + String.Format("[OperationContract]\n\t\t{0} {1} ({2});\n\t\t", function.ReturnType, function.Name, parametersValue);
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
            var value = Util.GetTemplate("TemplatesFile.Service.txt");

            // replace all tags, except body
            value = value.Replace("{namespace}", _namespaceValue);
            value = value.Replace("{serviceName}", _serviceName);
            value = value.Replace("{originService}", originService);

            string functionValue = String.Empty;
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();

            foreach (var function in functions)
            {
                string arguments = String.Empty;
                string parametersValue = String.Empty;
                string parametersCountString = String.Empty;
                int index = 0;

                // Creating template parameters
                for (int i = 0; i < function.Parameters.Count; i++)
                {
                    var type = function.Parameters[i].Type;
                    var comma = function.Parameters.Count == i + 1 ? String.Empty : ", ";
                    parametersValue = parametersValue + String.Format("{0} {1}{2}", type, alpha[i], comma);
                    parametersCountString = parametersCountString + "{" + (i + 2) + "}";
                    arguments = arguments + " " + alpha[i] + comma;
                    index++;
                }

                functionValue = functionValue + String.Format("public {0} {1} ({2})", function.ReturnType, function.Name, parametersValue) + "{\n";
                if (function.ReturnType == "void")
                {
                    var client = String.Format("_client.{0}(", function.Name)+ arguments + ");\n}\n\n";
                    functionValue = functionValue + client;
                }
                else
                {
                    var argFunction = String.IsNullOrEmpty(arguments) ? String.Empty : ", " + arguments;
                    var keyFunction = "var key = String.Format(\"{0}{1}" + parametersCountString + "\", \"" + _serviceName + "\",\"" + function.Name + "\"" + argFunction + ");\n";
                    var valueFunction = "var value = _db.StringGet(key);\n";
                    var ifFunction = " if(value.IsNullOrEmpty){\n";
                    var responseFunction = String.Format("var response = _client.{0}({1});", function.Name, arguments) + "\n";
                    var tempFunction = String.Format("_db.StringSet(key, ConverterType.ConvertToString<{0}>(response));", function.ReturnType) + "\nreturn response;\n}\n";
                    var elseFunction = String.Format("\nreturn ConverterType.ConvertFromString<{0}>(value);", function.ReturnType) + "\n}\n";
                    functionValue = functionValue + keyFunction + valueFunction + ifFunction + responseFunction + tempFunction + elseFunction;
                }
            }

            value = value.Replace("{body}", functionValue);

            var projectPath = _path + "/" + _serviceName + ".svc.cs";
            Util.CreateFile(projectPath, value);
        }

        public static void FileServiceSVC()
        {
            var value = Util.GetTemplate("TemplatesFile.ServiceSVC.txt");

            value = value.Replace("{namespace}", _namespaceValue);
            value = value.Replace("{serviceName}", _serviceName);

            // create new file in the path with string value

            var projectPath = _path + "/" + _serviceName + ".svc";
            Util.CreateFile(projectPath, value);
        }

        public static void FileProj(string originService, List<ObjectType> objectTypes)
        {
            var value = Util.GetTemplate("TemplatesFile.Proj.txt");

            value = value.Replace("{namespace}", _namespaceValue);
            value = value.Replace("{serviceName}", _serviceName);
            value = value.Replace("{originService}", originService);

            var projectPath = _path + "\\" + _serviceName + ".csproj";
            Util.CreateFile(projectPath, value);
        }

        public static void WebConfig()
        {
            var value = Util.GetTemplate("TemplatesFile.Webconfig.txt");

            value = value.Replace("{log-path}", _logPath);

            Util.CreateFile(_path + "/Web.config", value);
        }

        public static void Package()
        {
            var value = Util.GetTemplate("TemplatesFile.Package.txt");

            Util.CreateFile(_path + "/packages.config", value);
        }

        public static void Solution()
        {
            var value = Util.GetTemplate("TemplatesFile.Solution.txt");

            value = value.Replace("{serviceName}", _serviceName);

            var projectPath = _path + "\\" + _serviceName + ".sln";
            Util.CreateFile(projectPath, value);
        }

        public static void CreateProxyClassFromStream(ServiceObject service)
        {
            if (service.FileStream != null)
            {
                var filePath = service.Path + "\\proxy.wsdl";
                var file = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
                service.FileStream.Position = 0;
                service.FileStream.CopyTo(file);   
                file.Close();            
                service.Url = filePath;
            }
        }
    }
}
