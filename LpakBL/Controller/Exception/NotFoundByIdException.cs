namespace LpakBL.Controller.Exception
{
    /// <summary>
    /// Класс исключений возникающих при отсутствии элемента с указанным Id
    /// </summary>
    public class NotFoundByIdException : System.Exception
    {
        /// <summary>
        /// Конструктор класса <see cref="NotFoundByIdException"/>
        /// </summary>
        /// <param name="message">Текст сообщения об ошибка</param>
        public NotFoundByIdException(string message) : base(message){}
    }
}