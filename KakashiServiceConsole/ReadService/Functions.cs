using System;
using System.Collections.Generic;

namespace KakashiServiceConsole.ReadService
{
    public class Functions
    {
        public Functions()
        {
            ReturnType = TypeVariable.TypeVoid;
            Parameters = new List<Parameter>();
        }
        public String Name { get; set; }
        public TypeVariable ReturnType { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}
