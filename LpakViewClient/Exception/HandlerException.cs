using System;
using LpakBL.Controller.Exception;
using LpakBL.Model.Exception;

namespace LpakViewClient
{
    public static class HandlerException 
    {
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