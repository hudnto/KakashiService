using KakashiService.Core.Entities;
using System;
using System.ComponentModel;

namespace KakashiService.Web.ViewModel
{
    public class ConfigurationVM
    {
        [DisplayName("Service Name")]
        public String ServiceName { get; set; }
        [DisplayName("Number of the Port")]
        public int Port { get; set; }
        public String Namespace { get; set; }
        [DisplayName("Endpoint of the service that will be cloned")]
        public String Url { get; set; }
        [DisplayName("Directory of the source service")]
        public String BuildPath { get; set; }

        public ConfigurationVM()
        {

        }

        public ConfigurationVM(bool fake)
        {
            if (fake)
            {
                ServiceName = "t1";
                Port = 10001;
                Url = "http://www.dneonline.com/calculator.asmx?WSDL";
                BuildPath = "C:\\inetpub\\kakashi\\source\\t1";
                Namespace = "Kakashi";
            }
        }

        public static ServiceObject Convert(ConfigurationVM source)
        {
            var destin = new ServiceObject();

            destin.Name = source.ServiceName;
            destin.Port = source.Port;
            destin.Namespace = "Kakashi";
            destin.Url = source.Url;
            destin.Path = source.BuildPath;

            return destin;
        }
    }
}