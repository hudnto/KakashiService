namespace KakashiServiceConsole.ReadService
{
    public class Parameter
    {
        public Parameter()
        {

        }

        public Parameter(int order, string tipo)
        {
            Order = order;
            Type = Util.GetTipoVariavel(tipo);
        }

        public int Order { get; set; }
        public TypeVariable Type { get; set; }
    }
}