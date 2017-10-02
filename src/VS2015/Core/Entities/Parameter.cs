using System;
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

        public static Parameter GetElementFromWSDL(XElement xElement)
        {
            var element = new Parameter();
            element.Name = xElement.Attribute("name").Value;
            element.Type = NormalizeVariable(xElement.Attribute("type").Value);
            return element;
        }
    }
}