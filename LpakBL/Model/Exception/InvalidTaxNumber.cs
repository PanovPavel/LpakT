namespace LpakBL.Model.Exception
{
    public class InvalidTaxNumber : System.Exception
    {
        public override string HelpLink => @"http://www.regconsultgroup.ru/articles/chto-takoe-inn-ogrn-kpp/";

        public InvalidTaxNumber(string message) : base(message)
        {
        }

        public InvalidTaxNumber(string nameParameter, string message) : base(
            $"Name parameter: {nameParameter}. {message}")
        {
        }
    }
}