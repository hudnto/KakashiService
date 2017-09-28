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

        private void GetImportedFiles()
        {
            // check for import tags

            // add new xml document to the list
        }

        public void GetInfoFromService(ServiceObject serviceObject)
        {
            var xml = GetXml(serviceObject);

            _xmlDocs.Add(xml);            

            GetImportedFiles();              
        }

        //Methods for Parsing
    }
}
