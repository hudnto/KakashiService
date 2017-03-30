using System;

namespace KakashiService.Core.Entities
{
    public class Parameter
    {
        public Parameter(int order, string type)
        {
            Order = order;
            TypeName = type;
        }

        public int Order { get; set; }
        public String TypeName { get; set; }
    }
}