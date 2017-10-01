using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using KakashiService.Core.Entities;

namespace KakashiService.Core.Modules.Read
{
    public class ParseWsdl
    {
        public String ServiceAddress { get; }
        public List<Function> Functions { get; }

        public ParseWsdl(List<XmlDocument> xmls)
        {
            var xd = xmls.First().ToXDocument();
            Functions = new List<Function>();

            XNamespace wsdlNamespace = XNamespace.Get("http://schemas.xmlsoap.org/wsdl/");
            XNamespace xmlNamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema");

            var definitions = xd.Descendants(wsdlNamespace + "definitions");
            var types = definitions.Descendants(wsdlNamespace + "types");
            var portType = definitions.Descendants(wsdlNamespace + "portType");
            var operationsWSDL = portType.Descendants(wsdlNamespace + "operation");
            var messages = definitions.Descendants(wsdlNamespace + "message");
            var schemas = types.Descendants(xmlNamespace + "schema");
            var elements = schemas.Elements(xmlNamespace + "element");
            foreach (var operation in operationsWSDL)
            {
                var func = new Function();
                var name = operation.Attribute("name").Value;
                func.Name = name;

                var input = operation.Element(wsdlNamespace + "input").Attribute("message").Value.Split(':')[1];
                var output = operation.Element(wsdlNamespace + "output").Attribute("message").Value.Split(':')[1];
                var messageInput = messages.FirstOrDefault(a => a.Attribute("name").Value == input);
                var messageOutput = messages.FirstOrDefault(a => a.Attribute("name").Value == output);
                var elementNameInput = String.Empty;
                var elementNameOutput = String.Empty;
                if (messageInput != null && messageOutput != null)
                {
                    elementNameInput = messageInput.Element(wsdlNamespace + "part").Attribute("element").Value
                        .Split(':')[1];
                    elementNameOutput = messageOutput.Element(wsdlNamespace + "part").Attribute("element").Value
                        .Split(':')[1];
                }
                var elementInput = elements.FirstOrDefault(a => a.Attribute("name").Value == elementNameInput);
                var elementOutput = elements.FirstOrDefault(a => a.Attribute("name").Value == elementNameOutput);

                var index = 0;
                foreach (var element in elementInput.Descendants(xmlNamespace + "element"))
                {
                    var temp = Parameter.GetElementFromWSDL(element);
                    temp.Order = index;
                    func.Parameters.Add(temp);
                    index++;
                }
                var returnType = elementOutput.Descendants(xmlNamespace + "element").FirstOrDefault();
                if (returnType != null)
                {
                    var attribute = returnType.Attribute("type");

                    func.ReturnType = attribute == null ? "void" : Parameter.NormalizeVariable(attribute.Value);
                }
                else
                    func.ReturnType = "void";
                Functions.Add(func);
            }
            ServiceAddress = definitions.Descendants(wsdlNamespace + "service").First().Attribute("name").Value;
        }
    }
}