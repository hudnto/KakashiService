using KakashiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace KakashiService.Core.Modules.Create
{
    public class CreateFile
    {
        public static String _path;
        public static String _serviceName;
        public static String _serviceClientName;
        public static String _namespaceValue;
        public static String _logPath;

        public static void SetConfig(ServiceObject service)
        {
            _path = service.Path;
            _serviceName = service.Name;
            _namespaceValue = service.Namespace;
            _logPath = service.LogPath;
            _serviceClientName = service.ServiceClientName;
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

        public static void FileIService(List<Function> functions)
        {
            var value = Util.GetTemplate("TemplatesFile.Interface.txt");

            // replace all tags, except body
            value = value.Replace("{namespace}", _namespaceValue);
            value = value.Replace("{serviceName}", _serviceName);

            string functionValue = String.Empty;
            // replace body with functions
            foreach (var function in functions)
            {
                var parametersValue = String.Empty;
                int index = 0;
                for (int i = 0; i < function.Parameters.Count; i++)
                {
                    var name = function.Parameters[i].Name;
                    var type = function.Parameters[i].Type;
                    var comma = function.Parameters.Count == i + 1 ? String.Empty : ", ";
                    parametersValue = parametersValue + String.Format("{0} {1}{2}", type, name, comma);
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

        public static void FileService(List<Function> functions, String originService)
        {
            var value = Util.GetTemplate("TemplatesFile.Service.txt");

            // replace all tags, except body
            value = value.Replace("{namespace}", _namespaceValue);
            value = value.Replace("{serviceName}", _serviceName);
            value = value.Replace("{originService}", originService);
            value = value.Replace("{serviceClient}", _serviceClientName);

            string functionValue = String.Empty;
            try
            {
                foreach (var function in functions)
                {
                    string argumentsResponse = String.Empty;
                    string argumentsSimple = String.Empty;
                    string parametersValue = String.Empty;
                    string parametersCountString = String.Empty;
                    string initializeParameters = String.Empty;
                    int index = 0;

                    // Creating template parameters
                    for (int i = 0; i < function.Parameters.Count; i++)
                    {
                        var type = function.Parameters[i].Type;
                        var name = function.Parameters[i].Name;
                        var comma = function.Parameters.Count == i + 1 ? String.Empty : ", ";
                        parametersValue = parametersValue + String.Format("{0} {1}{2}", type, name, comma);
                        parametersCountString = parametersCountString + "{" + (i + 2) + "}";
                        if(type.Contains(" ") && type.Contains("out"))
                        {
                            var secondType = type.Split(' ')[1];
                            initializeParameters = "\n"+InitializeOut(name, secondType) +"\n"+initializeParameters;
                        }
                        argumentsSimple = argumentsSimple + String.Format("{0}{1}", name, comma);
                        if (type.Contains(" ") && type.Split(' ')[0] == "ref")
                            name = "ref " + name;
                        if (type.Contains(" ") && type.Split(' ')[0] == "out")
                            name = "out " + name;
                        argumentsResponse = argumentsResponse + String.Format("{0}{1}", name, comma);
                        index++;
                    }

                    functionValue = functionValue + String.Format("public {0} {1} ({2})", function.ReturnType, function.Name, parametersValue) + "{\n";
                    if (function.ReturnType == "void")
                    {
                        var client = String.Format("_client.{0}(", function.Name) + argumentsResponse + ");\n}\n\n";
                        functionValue = functionValue + client;
                    }
                    else
                    {
                        var argFunction = String.IsNullOrEmpty(argumentsSimple) ? String.Empty : ", " + argumentsSimple;
                        var keyFunction = "var key = String.Format(\"{0}{1}" + parametersCountString + "\", \"" + _serviceName + "\",\"" + function.Name + "\"" + argFunction + ");\n";
                        var valueFunction = "var value = _db.StringGet(key);\n";
                        var ifFunction = " if(value.IsNullOrEmpty){\n";
                        var responseFunction = String.Format("var response = _client.{0}({1});", function.Name, argumentsResponse) + "\n";
                        var tempFunction = String.Format("_db.StringSet(key, ConverterType.ConvertToString<{0}>(response));", function.ReturnType) + "\nreturn response;\n}\n";
                        var elseFunction = String.Format("\nreturn ConverterType.ConvertFromString<{0}>(value);", function.ReturnType) + "\n}\n";
                        functionValue = functionValue + initializeParameters + keyFunction + valueFunction + ifFunction + responseFunction + tempFunction + elseFunction;
                    }
                }


                value = value.Replace("{body}", functionValue);

                var projectPath = _path + "/" + _serviceName + ".svc.cs";
                Util.CreateFile(projectPath, value);
            }
            catch (Exception e)
            {
                throw new Exception("Error on Creating Service File", e);
            }
        }

        private static String InitializeOut(String name, String type)
        {
            var rightPart = " new "+type+"()";
            switch (type)
            {
                case "int": rightPart = "0";break;
                case "long": rightPart = "0f"; break;
                case "float": rightPart = "0.0"; break;
                case "decimal": rightPart = "0.0m"; break;
                case "string": rightPart = "String.Empty"; break;
            }
            return String.Format("{0} = {1};", name, rightPart);
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
