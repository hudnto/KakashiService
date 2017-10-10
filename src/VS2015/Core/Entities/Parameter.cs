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
            //var tempElements = element.Descendants(xmlNamespace + "element");
            //if (tempElements != null && tempElements.Count() > 0)
            //    element = tempElements.FirstOrDefault();   
            if (element == null || element.Attribute("type") == null)
                return null;

            var parameter = new Parameter();
            parameter.Name = element.Attribute("name").Value;
            parameter.Type = NormalizeVariable(element.Attribute("type").Value);

            return parameter;
        }
    }
}