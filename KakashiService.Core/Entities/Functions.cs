using System;
using System.Collections.Generic;

namespace KakashiService.Core.Entities
{
    public class Functions
    {
        public Functions()
        {
            ReturnType = "";
            Parameters = new List<Parameter>();
        }
        public String Name { get; set; }
        public String ReturnType { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}
