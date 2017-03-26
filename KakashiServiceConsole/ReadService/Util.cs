using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace KakashiServiceConsole.ReadService
{
    public static class Util
    {
        public static ServiceDescription GetServiceDescriptionSimple(string url)
        {
            XmlTextReader reader = new XmlTextReader(url);
            var serviceDescription = ServiceDescription.Read(reader);
            return serviceDescription;
        }

        public static ServiceDescription GetServiceDescriptionFromSVC(string url)
        {
            UriBuilder uriBuilder = new UriBuilder(url);
            uriBuilder.Query = "WSDL";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Method = "GET";
            webRequest.Accept = "text/xml";

            //Submit a web request to get the web service's WSDL
            ServiceDescription serviceDescription;
            using (WebResponse response = webRequest.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    serviceDescription = ServiceDescription.Read(stream);
                }
            }
            return serviceDescription;
        }

        public static XmlDocument DownloadXSD(string url)
        {
            XmlDocument myXmlDocument = new XmlDocument();
            myXmlDocument.Load(url);

            return myXmlDocument;
        }
        public static XmlSchemaSet GetAllSchema(ServiceDescription serviceDescription)
        {
            var schemaSet = new XmlSchemaSet();
            foreach (XmlSchema schema in serviceDescription.Types.Schemas)
            {
                if (schema.Includes.Count == 0)
                {
                    schemaSet.Add(schema);
                }
                else
                {
                    foreach (var include in schema.Includes)
                    {
                        var schemaLocation = ((XmlSchemaImport)include).SchemaLocation;
                        var xmlReader = new XmlTextReader(schemaLocation);
                        schemaSet.Add(schema.SourceUri, xmlReader);
                    }
                }
            }

            return schemaSet;
        }

        public static List<String> GetOperations(ServiceDescription serviceDescription)
        {
            var retorno = new List<String>();

            var messages = serviceDescription.Messages;
            foreach (Message message in messages)
            {
                var parts = message.Parts;
                foreach (MessagePart part in parts)
                {
                    retorno.Add(part.Element.Name);
                }
            }

            return retorno;
        }

        public static List<Functions> ExtractFunctionFromXml(List<XmlDocument> xmlDocuments, List<String> operacoes)
        {
            var functions = new List<Functions>();
            foreach (var xmlDocument in xmlDocuments)
            {
                var xd = XDocument.Parse(xmlDocument.InnerXml);

                var schema = xmlDocument.DocumentElement.NamespaceURI;
                var prefix = xmlDocument.DocumentElement.Prefix;

                List<XElement> elements =
                    (from node in xd.Descendants("{" + schema + "}element") where node.HasElements select node).ToList();

                foreach (var operacao in operacoes.Where(a => !a.Contains("Response")))
                {
                    var funcao = new Functions();
                    funcao.Name = operacao;

                    int ordem = 0;
                    foreach (var element in elements)
                    {
                        var elementos = from item2 in element.Descendants("{" + schema + "}element") select item2;
                        // define o tipo de retorno
                        if (element.Attribute("name").Value.Contains(operacao + "Response"))
                        {
                            if (elementos.Any())
                            {
                                var tipo = elementos.First().Attribute("type").Value.Replace(prefix + ":", String.Empty);
                                funcao.ReturnType = GetVariableType(tipo);
                            }
                            else
                            {
                                funcao.ReturnType = TypeVariable.TypeVoid;
                            }
                        }
                        // define os parametros de entrada
                        else if (element.Attribute("name").Value.Contains(operacao))
                        {
                            funcao.Parameters = new List<Parameter>();
                            if (elementos.Any())
                            {
                                foreach (var elemento in elementos)
                                {
                                    var tipo = elemento.Attribute("type").Value.Replace(prefix + ":", String.Empty);
                                    funcao.Parameters.Add(new Parameter(ordem, tipo));
                                    ordem++;
                                }
                            }
                        }
                    }
                    functions.Add(funcao);
                }
            }

            return functions;
        }

        public static TypeVariable GetVariableType(String tipo)
        {
            if (tipo == "string")
            {
                return TypeVariable.TypeString;
            }
            if (tipo == "int")
            {
                return TypeVariable.TypeInt;
            }
            return TypeVariable.TypeVoid;
        }

        public static string GetDescription(this Enum enumVal)
        {
            var type = enumVal.GetType();
            var attribute = type.GetMember(enumVal.ToString())[0]
                .GetCustomAttributes(false)
                .OfType<DescriptionAttribute>()
                .SingleOrDefault();

            var description = (attribute != null) ? attribute.Description : string.Empty;

            return description;
        }

        public static List<Functions> ExtractItemXml(XmlSchemaSet schemaSet)
        {
            var functionsResponse = new List<Functions>();
            var functions = new List<Functions>();
            foreach (XmlSchema xmlSchema in schemaSet.Schemas())
            {
                //TODO Make a better solution
                if (!xmlSchema.TargetNamespace.Contains("http://tempuri.org"))
                    continue;
                foreach (object item in xmlSchema.Items)
                {
                    var function = new Functions();
                    var functionResponse = new Functions();
                    XmlSchemaElement schemaElement = item as XmlSchemaElement;
                    XmlSchemaComplexType complexType = item as XmlSchemaComplexType;

                    if (schemaElement != null)
                    {
                        if (schemaElement.Name.Contains("Response"))
                        {
                            functionResponse.Name = schemaElement.Name;
                        }
                        else
                        {
                            function.Name = schemaElement.Name;
                        }
                        XmlSchemaType schemaType = schemaElement.SchemaType;
                        XmlSchemaComplexType schemaComplexType = schemaType as XmlSchemaComplexType;

                        if (schemaComplexType != null)
                        {
                            XmlSchemaParticle particle = schemaComplexType.Particle;
                            XmlSchemaSequence sequence = particle as XmlSchemaSequence;

                            if (sequence != null)
                            {
                                var index = 0;
                                foreach (XmlSchemaElement childElement in sequence.Items)
                                {
                                    if (schemaElement.Name.Contains("Response"))
                                    {
                                        functionResponse.ReturnType = GetVariableType(childElement.SchemaTypeName.Name);
                                    }
                                    else
                                    {
                                        function.Parameters.Add(new Parameter(index, childElement.SchemaTypeName.Name));
                                        index++;
                                    }
                                }

                            }
                            if (schemaComplexType.AttributeUses.Count > 0)
                            {
                                IDictionaryEnumerator enumerator = schemaComplexType.AttributeUses.GetEnumerator();

                                while (enumerator.MoveNext())
                                {
                                    XmlSchemaAttribute attribute = (XmlSchemaAttribute)enumerator.Value;

                                    Console.Out.WriteLine("      Attribute/Type: {0}", attribute.Name);
                                }
                            }
                        }
                        if (schemaElement.Name.Contains("Response"))
                        {
                            functionsResponse.Add(functionResponse);
                        }
                        else
                        {
                            functions.Add(function);
                        }
                    }
                    else if (complexType != null)
                    {
                        Console.Out.WriteLine("Complex Type: {0}", complexType.Name);
                        OutputElements(complexType.Particle);
                        if (complexType.AttributeUses.Count > 0)
                        {
                            IDictionaryEnumerator enumerator = complexType.AttributeUses.GetEnumerator();

                            while (enumerator.MoveNext())
                            {
                                XmlSchemaAttribute attribute = (XmlSchemaAttribute)enumerator.Value;
                                Console.Out.WriteLine("      Attribute/Type: {0}", attribute.Name);
                            }
                        }
                    }
                }
            }

            foreach (var response in functionsResponse)
            {
                var functionName = response.Name.Replace("Response", "");
                var function = functions.First(a => a.Name == functionName);
                function.ReturnType = response.ReturnType;
            }

            return functions;
        }

        private static void OutputElements(XmlSchemaParticle particle)
        {
            XmlSchemaSequence sequence = particle as XmlSchemaSequence;
            XmlSchemaChoice choice = particle as XmlSchemaChoice;
            XmlSchemaAll all = particle as XmlSchemaAll;

            if (sequence != null)
            {
                Console.Out.WriteLine("  Sequence");

                for (int i = 0; i < sequence.Items.Count; i++)
                {
                    XmlSchemaElement childElement = sequence.Items[i] as XmlSchemaElement;

                    if (childElement != null)
                    {
                        Console.Out.WriteLine("    Element/Type: {0}:{1}", childElement.Name,
                                              childElement.SchemaTypeName.Name);
                    }
                    else OutputElements(sequence.Items[i] as XmlSchemaParticle);
                }
            }
            else if (choice != null)
            {
                Console.Out.WriteLine("  Choice");
                for (int i = 0; i < choice.Items.Count; i++)
                {
                    XmlSchemaElement childElement = choice.Items[i] as XmlSchemaElement;

                    if (childElement != null)
                    {
                        Console.Out.WriteLine("    Element/Type: {0}:{1}", childElement.Name,
                                              childElement.SchemaTypeName.Name);
                    }
                    else OutputElements(choice.Items[i] as XmlSchemaParticle);
                }

                Console.Out.WriteLine();
            }
            else if (all != null)
            {
                Console.Out.WriteLine("  All");
                for (int i = 0; i < all.Items.Count; i++)
                {
                    XmlSchemaElement childElement = all.Items[i] as XmlSchemaElement;

                    if (childElement != null)
                    {
                        Console.Out.WriteLine("    Element/Type: {0}:{1}", childElement.Name,
                                              childElement.SchemaTypeName.Name);
                    }
                    else OutputElements(all.Items[i] as XmlSchemaParticle);
                }
                Console.Out.WriteLine();
            }
        }
    }
}
