using KakashiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KakashiService.Core.Modules.Create
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
            // Get Resource file 
            var fileName = "Interface.txt";
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
                    var type = function.Parameters[i].TypeName;
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
            // Get Resource file 
            var fileName = "Service.txt";
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
                for (int i = 0; i < function.Parameters.Count; i++)
                {
                    var type = function.Parameters[i].TypeName;
                    var comma = function.Parameters.Count == i + 1 ? String.Empty : ", ";
                    parametersValue = parametersValue + String.Format("{0} {1}{2}", type, alpha[i], comma);
                    arguments = arguments + alpha[i] + " " + comma;
                    index++;
                }

                functionValue = functionValue + String.Format("public {0} {1} ({2})", function.ReturnType, function.Name, parametersValue);
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

        public static void FileDataContract(List<ObjectType> objectTypes)
        {
            // Get Resource file 
            var fileName = "DataContract.txt";
            var assembly = Assembly.GetExecutingAssembly();
            var allResources = assembly.GetManifestResourceNames();
            var resourceName = allResources.First(a => a.Contains(fileName));

            var value = String.Empty;
            foreach (var objectType in objectTypes)
            {


                // read model in txt
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    value = reader.ReadToEnd();
                }

                // replace all tags, except body
                value = value.Replace("{namespace}", _namespaceValue);
                value = value.Replace("{object_name}", objectType.TypeName);

                // create attributes with datamember 
                char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();
                var index = 0;
                var attributesTemplate = String.Empty;
                foreach (var attribute in objectType.Attributes)
                {
                    var template = String.Format("\n[DataMember]\npublic {0} {1} {{get;set;}}\n", attribute, alpha[index]);
                    index++;
                    attributesTemplate += template;
                }
                value = value.Replace("{attributes}", attributesTemplate);

                // create initialization of parameters constructor
                // and create the body of the constructor

                var parametersTemplate = String.Empty;
                var initializationTemplate = String.Empty;

                for (int i = 0; i < objectType.Attributes.Count; i++)
                {
                    // is the last? no comma
                    var comma = i + 1 == objectType.Attributes.Count ? String.Empty : ",";
                    var template = String.Format("{0} {1}{2}", objectType.Attributes[i], alpha[i], comma);
                    parametersTemplate += template;

                    var templateInit = String.Format("\nthis.{0} = {1};\n", alpha[i], alpha[i]);

                    initializationTemplate += templateInit;
                }

                value = value.Replace("{parameter_constructor}", parametersTemplate);
                value = value.Replace("{initialization}", initializationTemplate);

                FileInfo file = new FileInfo(_path + "/" + objectType.TypeName + ".cs");
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

        public static void FileServiceSVC()
        {
            // Get Resource file 
            var fileName = "ServiceSVC.txt";
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

        public static void FileProj(string originService, List<ObjectType> objectTypes)
        {
            // Get Resource file 
            var fileName = "Proj.txt";
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

            value = value.Replace("{namespace}", _namespaceValue);
            value = value.Replace("{serviceName}", _serviceName);
            value = value.Replace("{originService}", originService);

            var objectTemplate = String.Empty;
            foreach (var objectType in objectTypes)
            {
                var include = String.Format("<Compile Include=\"{0}.cs\" />", objectType.TypeName);
                objectTemplate += include;
            }
            value = value.Replace("{objects_types}", objectTemplate);


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
            // Get Resource file 
            var fileName = "Webconfig.txt";
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
