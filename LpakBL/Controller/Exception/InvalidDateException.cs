namespace LpakBL.Controller.Exception
{
    /// <summary>
    /// Класс исключений, связанных с недопустимыми датами.
    /// </summary>
    public class InvalidDateException:System.Exception
    {
        /// <summary>
        /// Конструктор класса <see cref="InvalidDateException"/>.
        /// </summary>
        /// <param name="message">Устанавливет текст исключения</param>
        public InvalidDateException(string message) : base(message){}
    }
}