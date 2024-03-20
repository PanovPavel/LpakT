namespace LpakBL.Model.Exception
{
    /// <summary>
    /// Класс исключение возникающего при неправильной длинне значения
    /// </summary>
    public class IncorrectLongOrNullException : System.Exception
    {
        /// <summary>
        /// Конструктор класса IncorrectLongOrNullException
        /// </summary>
        /// <param name="message">текст сообщения об ошибке</param>
        public IncorrectLongOrNullException(string message) : base(message)
        {
        }
        /// <summary>
        /// Конструктор класса IncorrectLongOrNullException
        /// </summary>
        /// <param name="message">текст сообщения об ошибке</param>
        public IncorrectLongOrNullException(string nameParameter, string message) : base(
            $"Name parameter: {nameParameter}. {message}")
        {
        }
    }
}