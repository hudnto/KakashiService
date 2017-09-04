using System;

namespace KakashiService.Core.Entities
{
    public class AttributeType
    {
        public String Name { get; set; }
        public int Order { get; set; }

        public AttributeType()
        {

        }
        public AttributeType(string typeName)
        {
            Name = typeName;
        }
        public AttributeType(int order, string typeName)
        {
            Order = order;
            Name = typeName;
        }
    }
}