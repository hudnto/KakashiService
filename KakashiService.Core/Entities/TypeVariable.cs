using System.ComponentModel;

namespace KakashiService.Core.Entities
{
    public enum TypeVariable
    {
        [Description("string")]
        TypeString,
        [Description("int")]
        TypeInt,
        [Description("float")]
        TypeFloat,
        [Description("datetime")]
        TypeDatetime,
        [Description("void")]
        TypeVoid
    }
}