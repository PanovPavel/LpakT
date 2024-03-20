using System;
using LpakBL.Controller.Exception;
using LpakBL.Model.Exception;

namespace LpakViewClient.Exceptions
{
    /// <summary>
    /// Класса для обработки исключений возникающих в model и controller
    /// </summary>
    public static class HandlerException 
    {
        /// <summary>
        /// Проверяет принадлежит ли исключение к конкретным типам
        /// </summary>
        /// <param name="ex">Объект исключения</param>
        /// <returns>Возвращает true если ex принадлежит указонному списку исключений. Если нет false </returns>
        public static bool IsHandledException(Exception ex)
        {
            return ex is IncorrectLongOrNullException ||
                   ex is InvalidTaxNumber ||
                   ex is InvalidDateException ||
                   ex is NotFoundByIdException ||
                   ex is RelatedRecordsException ||
                   ex is UniquenessStatusException;
        }
    }
}