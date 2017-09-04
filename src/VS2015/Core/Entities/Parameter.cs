using System;
using KakashiService.Core.Modules;
using KakashiService.Core.Modules.Read;

namespace KakashiService.Core.Entities
{
    public class Parameter
    {
        public Parameter(int order, string type)
        {
            Order = order;
            Type = Util.NormalizeVariable(type);
        }

        public int Order { get; set; }
        public string Type { get; set; }
    }
}