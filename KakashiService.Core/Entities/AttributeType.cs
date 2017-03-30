using System;

namespace KakashiService.Core.Entities
{
    public class AttributeType
    {
        public String Name { get; set; }
        public TypeVariable Type { get; set; }
        public int Order { get; set; }

        public AttributeType()
        {

        }

        public AttributeType(int order, string typeName)
        {
            Order = order;
            Name = typeName;
            Type = GetVariableType(typeName);
        }

        public AttributeType(string typeName)
        {
            Name = typeName;
            Type = GetVariableType(typeName);
        }

        public static TypeVariable GetVariableType(String tipo)
        {
            if (tipo == "string")
            {
                return TypeVariable.TypeString;
            }
            if (tipo == "int")
            {
                return TypeVariable.TypeInt;
            }
            if (tipo == "float")
            {
                return TypeVariable.TypeFloat;
            }
            if (tipo == "datetime")
            {
                return TypeVariable.TypeDatetime;
            }
            return TypeVariable.TypeVoid;
        }
    }
}