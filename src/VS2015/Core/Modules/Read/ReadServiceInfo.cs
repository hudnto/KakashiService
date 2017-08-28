using KakashiService.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Services.Description;
using System.Xml.Schema;

namespace KakashiService.Core.Modules.Read
{
    public class ReadServiceInfo
    {
        private List<ObjectType> _objectTypes;

        public ReadServiceInfo()
        {
            _objectTypes = new List<ObjectType>();
        }

        private void AddObject(ObjectType objectType)
        {
            _objectTypes.Add(objectType);
        }

        public List<ObjectType> GetObjectTypes()
        {
            return _objectTypes;
        }

        public ServiceDescription GetServiceDescriptionFromObject(ServiceObject objectService)
        {
            try
            {
                if (objectService.FileStream != null)
                    return ServiceDescription.Read(objectService.FileStream);

                UriBuilder uriBuilder = new UriBuilder(objectService.Url);
                uriBuilder.Query = "WSDL";

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Method = "GET";
                webRequest.Accept = "text/xml";

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
            catch (WebException e)
            {
                throw new WebException("Error while reading service. Check if it is online.", e);
            }
        }

        public XmlSchemaSet GetAllSchema(ServiceDescription serviceDescription)
        {
            var schemaSet = new XmlSchemaSet();
            foreach (XmlSchema schema in serviceDescription.Types.Schemas)
            {
                schemaSet.Add(schema);

                //if (schema.Includes.Count == 0)
                //{
                //    schemaSet.Add(schema);
                //}
                //else
                //{
                //    foreach (var include in schema.Includes)
                //    {
                //        var schemaLocation = ((XmlSchemaImport)include).SchemaLocation;
                //        schemaSet.Add(schema.SourceUri, schemaLocation);
                //    }
                //}
            }

            return schemaSet;
        }

        public List<Functions> ExtractItemXml(XmlSchemaSet schemaSet)
        {
            var functionsResponse = new List<Functions>();
            var functions = new List<Functions>();
            foreach (XmlSchema xmlSchema in schemaSet.Schemas())
            {
                ////TODO Make a better solution
                if (xmlSchema.TargetNamespace.Contains("Serialization"))
                    continue;
                foreach (object item in xmlSchema.Items)
                {
                    var function = new Functions();
                    var functionResponse = new Functions();
                    XmlSchemaElement schemaElement = item as XmlSchemaElement;
                    XmlSchemaComplexType complexType = item as XmlSchemaComplexType;
                    var index = 0;

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
                                foreach (XmlSchemaObject teste in sequence.Items)
                                {
                                    XmlSchemaElement childElement = teste as XmlSchemaElement;
                                    XmlSchemaAny childElement2 = teste as XmlSchemaAny;
                                    var childElement3 = teste.GetType();

                                    if (childElement == null)
                                        continue;

                                    if (schemaElement.Name.Contains("Response"))
                                    {
                                        functionResponse.ReturnType = childElement.SchemaTypeName.Name;
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
                            if (schemaElement.Name.Contains("Response"))
                            {
                                functionsResponse.Add(functionResponse);
                            }
                            else
                            {
                                functions.Add(function);
                            }
                        }

                    }
                    else if (complexType != null)
                    {
                        Console.Out.WriteLine("Complex Type: {0}", complexType.Name);
                        var objectType = new ObjectType(index, complexType.Name);
                        OutputElements(complexType.Particle, objectType);
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

        private void OutputElements(XmlSchemaParticle particle, ObjectType objectType)
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
                    XmlSchemaComplexType childComplexType = sequence.Items[i] as XmlSchemaComplexType;

                    if (childElement != null)
                    {
                        Console.Out.WriteLine("    Element/Type: {0}:{1}", childElement.Name,
                            childElement.SchemaTypeName.Name);
                        objectType.Attributes.Add(childElement.SchemaTypeName.Name);
                    }
                    else
                    {
                        var name = childComplexType.Name;
                        OutputElements(sequence.Items[i] as XmlSchemaParticle, new ObjectType(0, name));
                    }
                }
                AddObject(objectType);
            }
        }
    }
}
