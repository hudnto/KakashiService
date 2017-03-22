using System;
using System.Collections.Generic;

namespace KakashiServiceConsole.ReadService
{
    public class ServiceObject
    {
        public String Name { get; set; }
        public String Path { get; set; }
        public String Url { get; set; }

        public String Namespace { get; set; }
        public int Port { get; set; }

        public String SvcUtilPath { get; set; }
        public String MsBuildPath { get; set; }

        public String IISPath { get; set; }
        public String OriginServiceName { get; set; }
        public List<Functions> Functions { get; set; }
    }
}
