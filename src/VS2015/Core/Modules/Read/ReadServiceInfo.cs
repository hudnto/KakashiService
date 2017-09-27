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

        public XmlDocument GetXmlFromStream(FileStream stream)
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

        public XmlDocument GetXmlFromUrl(String url)
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

        public XmlDocument GetXml(ServiceObject serviceObject)
        {
            return new XmlDocument();
        }

        public void GetImportedFiles()
        {
            // check if it needs to import files

            // update xml document 
        }

        public List<XmlDocument> GetAllXML(ServiceObject serviceObject)
        {
            var xml = GetXml(serviceObject);

            _xmlDocs.Add(xml);

            GetImportedFiles();

            return _xmlDocs;
        }
    }
}
