using System.ComponentModel;

namespace KakashiService.Core.Entities
{
    public enum TypeVariable
    {
        [Description("string")]
        TypeString,
        [Description("int")]
        TypeInt,
        [Description("void")]
        TypeVoid
    }
}