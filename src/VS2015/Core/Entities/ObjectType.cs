using System.Collections.Generic;

namespace KakashiService.Core.Entities
{
    public class ObjectType : Parameter
    {
        public List<string> Attributes { get; set; }

        public ObjectType(int order, string type)
            : base(order, type)
        {
            Attributes = new List<string>();
        }
    }
}