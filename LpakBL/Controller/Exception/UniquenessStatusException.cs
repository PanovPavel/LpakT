namespace LpakBL.Controller.Exception
{
    /// <summary>
    /// Класс исключения, возникающего при нарушении уникальных значений в базе данных.
    /// </summary>
    public class UniquenessStatusException : System.Exception
    {
        /// <summary>
        /// Конструктор класса <see cref="UniquenessStatusException"/>.
        /// </summary>
        /// <param name="message">текст сообщения об ошибке</param>
        public UniquenessStatusException(string message) : base(message){}
    }
}