using System;
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
        public static XmlDocument GetXsdInicial(ServiceDescription serviceDescription)
        {
            var xsdSchema = serviceDescription.Types.Schemas.First().Includes[0];
            var xsd = ((XmlSchemaImport)xsdSchema).SchemaLocation;
            var xmlDocument = Util.DownloadXSD(xsd);
            return xmlDocument;
        }

        public static List<String> BuscarOperacoes(ServiceDescription serviceDescription)
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

        public static List<Functions> ExtrairFuncaoXml(XmlDocument xmlDocument, List<String> operacoes)
        {
            var retorno = new List<Functions>();
            var xd = XDocument.Parse(xmlDocument.InnerXml);

            var schema = xmlDocument.DocumentElement.NamespaceURI;
            var prefix = xmlDocument.DocumentElement.Prefix;

            List<XElement> elements = (from node in xd.Descendants("{" + schema + "}element") where node.HasElements select node).ToList();

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
                            funcao.ReturnType = GetTipoVariavel(tipo);
                        }
                        else
                        {
                            funcao.ReturnType = TypeVariable.TypeVoid;
                        }
                    }
                    // define os parametros de entrada
                    else if (element.Attribute("name").Value.Contains(operacao))
                    {
                        funcao.Parametros = new List<Parameter>();
                        if (elementos.Any())
                        {
                            foreach (var elemento in elementos)
                            {
                                var tipo = elemento.Attribute("type").Value.Replace(prefix + ":", String.Empty);
                                funcao.Parametros.Add(new Parameter(ordem, tipo));
                                ordem++;
                            }
                        }
                    }
                }
                retorno.Add(funcao);
            }

            return retorno;
        }

        public static TypeVariable GetTipoVariavel(String tipo)
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
    }
}
