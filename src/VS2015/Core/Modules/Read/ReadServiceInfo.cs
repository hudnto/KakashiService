using KakashiService.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace KakashiService.Core.Modules.Read
{
    public class ReadServiceInfo
    {
        private List<XmlDocument> _xmlDocs;

        public ReadServiceInfo()
        {
            _xmlDocs = new List<XmlDocument>();
        }

        private XmlDocument GetXmlFromStream(Stream stream)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                if (stream != null)
                {
                    xmlDoc.Load(stream);
                }
                return xmlDoc;
            }
            catch (WebException e)
            {
                throw new WebException("Error while reading service. Check if it is online.", e);
            }
            catch (Exception e)
            {
                throw new WebException("Error while reading service. General error", e);
            }
        }

        private XmlDocument GetXmlFromUrl(String url)
        {
            var xml = new XmlDocument();
            try
            {
                UriBuilder uriBuilder = new UriBuilder(url);
                uriBuilder.Query = "WSDL";

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Method = "GET";
                webRequest.Accept = "text/xml";

                using (WebResponse response = webRequest.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        xml.Load(stream);
                    }
                }
                return xml;
            }
            catch (WebException e)
            {
                throw new WebException("Error while reading service. Check if it is online.", e);
            }
            catch (Exception e)
            {
                throw new WebException("Error while reading service. General error", e);
            }
        }

        private XmlDocument GetXml(ServiceObject serviceObject)
        {
            if(serviceObject.FileStream == null)
            {
                return GetXmlFromUrl(serviceObject.Url);
            }
            else
            {
                return GetXmlFromStream(serviceObject.FileStream);
            }            
        }

        private void AddImportedFiles()
        {
            // check for import tags

            // add new xml document to the list
        }

        public void GetInfoFromService(ServiceObject serviceObject)
        {
            var xml = GetXml(serviceObject);

            _xmlDocs.Add(xml);

            AddImportedFiles();

            var parseWsdl = new ParseWSDL(_xmlDocs);
                                 
        }        
    }

    public class ParseWSDL
    {
        public List<OperationWSDL> Operations { get; private set; }
        public List<MessageWSDL> Messages { get; private set; }
        public List<ElementWSDL> Schemas { get; private set; }
        public String ServiceAddress { get; private set; }

        public ParseWSDL(List<XmlDocument> _xmls)
        {
            Operations = new List<OperationWSDL>();
            Messages = new List<MessageWSDL>();
            Schemas = new List<ElementWSDL>();
            GetDataFromWSDL(_xmls);
        }

        public void GetDataFromWSDL(List<XmlDocument> xmls)
        {
            // merge all Xml in One
            var xml = xmls.First();
            
            // Get operations from PortType
            foreach(XElement xn in portType)
            {

            }

            // Get messages using operations

        }        
    }

    public class OperationWSDL
    {
        public String Name { get; set; }
        public String Input { get; set; }
        public String Output { get; set; }
    }

    public class MessageWSDL
    {
        public String Name { get; set; }
        public String Element { get; set; }        
    }

    public class ElementWSDL
    {

    }
}
