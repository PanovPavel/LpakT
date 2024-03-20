namespace LpakBL.Model.Exception
{

    /// <summary>
    /// Класс исключений, возникающих при невалидном значении налогового номера
    /// </summary>
    public class InvalidTaxNumber : System.Exception
    {
        
        /// <summary>
        /// Ресурс со справочной информацией о валидности номера налогообложения.
        /// </summary>
        public override string HelpLink => @"https://www.regconsultgroup.ru/articles/chto-takoe-inn-ogrn-kpp/";
        
        /// <summary>
        /// Конструктор класса InvalidTaxNumber
        /// </summary>
        /// <param name="message">текст сообщения об ошибке</param>
        public InvalidTaxNumber(string message) : base(message)
        {
        }
        /// <summary>
        /// Конструктор класса InvalidTaxNumber
        /// </summary>
        /// <param name="message">текст сообщения об ошибке</param>
        public InvalidTaxNumber(string nameParameter, string message) : base(
            $"Name parameter: {nameParameter}. {message}")
        {
        }
    }
}