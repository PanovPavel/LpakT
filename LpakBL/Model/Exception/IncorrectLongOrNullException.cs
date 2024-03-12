namespace LpakBL.Model.Exception
{
    public class IncorrectLongOrNullException : System.Exception
    {
        public IncorrectLongOrNullException(string message) : base(message)
        {
        }

        public IncorrectLongOrNullException(string nameParameter, string message) : base(
            $"Name parameter: {nameParameter}. {message}")
        {
        }
    }
}