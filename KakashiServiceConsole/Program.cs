using KakashiServiceConsole.BuildService;
using KakashiServiceConsole.CreateService;
using KakashiServiceConsole.ReadService;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace KakashiServiceConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceObject = new ServiceObject();

            serviceObject.Name = "DividirParaConquistar6";
            serviceObject.Port = 20018;
            serviceObject.Path = @"C:\Users\lcramos1\Desktop\Kakashi";
            serviceObject.Namespace = "Kakashi";

            serviceObject.Url = "http://localhost:40799/ServicoData.svc?wsdl";//"http://www.dneonline.com/calculator.asmx?wsdl";

            serviceObject.IISPath = ConfigurationManager.AppSettings["iisPath"];
            serviceObject.MsBuildPath = ConfigurationManager.AppSettings["msbuildPath"];
            serviceObject.SvcUtilPath = ConfigurationManager.AppSettings["svcutilPath"];

            ReadService(serviceObject);

            CreateService(serviceObject);

            BuildService(serviceObject);
        }
        private static ServiceObject FuncaoFake()
        {
            var service = new ServiceObject();
            var lista = new List<Functions>()
                {
                    new Functions()
                    {
                        Name = "FuncaoData",
                        Parametros = new List<Parameter>() {},
                        ReturnType = TypeVariable.TypeString
                    },
                    new Functions()
                    {
                        Name = "FuncaoData2",
                        Parametros = new List<Parameter>()
                        {
                            new Parameter(){Order = 1, Type = TypeVariable.TypeInt},
                            new Parameter(){Order = 2, Type = TypeVariable.TypeInt}
                        },
                        ReturnType = TypeVariable.TypeInt
                    }
                };
            service.Functions = lista;
            return service;
        }

        private static void ReadService(ServiceObject serviceObject)
        {
            var serviceDescription = Util.GetServiceDescriptionFromSVC(serviceObject.Url);

            var xsd = Util.GetXsdInicial(serviceDescription);

            var operacoes = Util.BuscarOperacoes(serviceDescription);

            serviceObject.Functions = Util.ExtrairFuncaoXml(xsd, operacoes);
            serviceObject.OriginServiceName = serviceDescription.Name;
        }

        public static void CreateService(ServiceObject service)
        {
            CreateFile.SetConfig(service.Path, service.Name, service.Namespace);
            CreateFile.FileIService(service.Functions);
            CreateFile.FileService(service.Functions, service.OriginServiceName);
            CreateFile.FileServiceSVC();
            CreateFile.FileProj(service.OriginServiceName);
            CreateFile.WebConfig();
        }

        private static void BuildService(ServiceObject service)
        {
            BuildTemplate.CreateProxyClass(service);

            var projectPath = String.Format("{0}/{1}.csproj", service.Path, service.Name);

            BuildTemplate.Build(projectPath, service.MsBuildPath);

            BuildSite.Create(service.Name, service.Port, service.IISPath);

            var binWebSitePath = String.Format("{0}/{1}", service.IISPath, service.Name);

            BuildTemplate.MoveBin(service.Path, binWebSitePath);
        }
    }
}
