
namespace LpakBL.Controller.Exception
{
    /// <summary>
    /// Класс исключения, которое возникает при попытке удалить объект, у которого есть связь с другими данными в базе данных.
    /// </summary>
    public class RelatedRecordsException:System.Exception
    {
        /// <summary>
        /// Конструктор класса <see cref="RelatedRecordsException"/>.
        /// </summary>
        /// <param name="message">устанавливает текст исключения</param>
        public RelatedRecordsException(string message) : base(message){}
    }
}