using System;
using System.Collections.Generic;

namespace KakashiService.Core.Entities
{
    public class Function
    {
        public Function()
        {
            ReturnType = "";
            Parameters = new List<Parameter>();
        }
        public String Name { get; set; }
        public String ReturnType { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}
