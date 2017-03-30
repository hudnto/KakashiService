using KakashiService.Core.Entities;
using System;
using System.ComponentModel;
using System.Linq;

namespace KakashiService.Core.Modules.Read
{
    public static class Util
    {
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
            return TypeVariable.TypeVoid;
        }

        public static string GetDescription(this Enum enumVal)
        {
            var type = enumVal.GetType();
            var attribute = type.GetMember(enumVal.ToString())[0]
                .GetCustomAttributes(false)
                .OfType<DescriptionAttribute>()
                .SingleOrDefault();

            var description = (attribute != null) ? attribute.Description : string.Empty;

            return description;
        }
    }
}
