using System;
using System.Linq;
using System.Xml.Linq;

namespace KakashiService.Core.Entities
{
    public class Parameter
    {
        public Parameter()
        {
            Order = 0;
        }

        public Parameter(int order, string type)
        {
            Order = order;
            Type = NormalizeVariable(type);
        }

        public String Name { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }

        public static String NormalizeVariable(String variable)
        {
            if (variable.Contains(":"))
            {
                variable = variable.Split(':')[1];
            }
            switch (variable)
            {
                case "dateTime":
                case "datetime": return "DateTime";
                case "boolean": return "bool";
                case "guid": return "Guid";
                case "": return "void";
            }
            return variable;
        }

        public static Parameter GetElementFromWSDL(XElement element, XNamespace xmlNamespace)
        {
            if (element == null)
                return null;
            var parameter = new Parameter();
            if (element.Attribute("type") == null)
            {
                parameter.Name = element.Attribute("name").Value;
                var appInfo = element.Descendants(xmlNamespace+ "appinfo").FirstOrDefault();
                var type = appInfo.Descendants().FirstOrDefault();
                var typeName = NormalizeVariable(type.Attribute("Name").Value);
                var namespaceType = type.Attribute("Namespace").Value.Split('/').Last();
                parameter.Type = namespaceType + "." + typeName;
            }
            else
            {
                parameter.Name = element.Attribute("name").Value;
                parameter.Type = NormalizeVariable(element.Attribute("type").Value);
            }
            return parameter;
        }
    }
}