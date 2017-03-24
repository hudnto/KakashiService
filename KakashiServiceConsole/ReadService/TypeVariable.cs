using System.ComponentModel;

namespace KakashiServiceConsole.ReadService
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