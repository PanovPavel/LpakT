using System;
using System.Runtime.Serialization;

namespace LpakBL.Model.Exception
{
    public class IncorrectLongException:System.Exception
    {
        public IncorrectLongException(string message) : base(message){ }
        public IncorrectLongException(string nameParameter, string message) : base($"Name parameter: {nameParameter}. {message}"){}
    }
}