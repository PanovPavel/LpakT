namespace LpakBL.Model.NumberCompanyValidator
{
    /// <summary>
    /// Валидатор налогового номера компании.
    /// </summary>
    public interface InnValidator
    {
        /// <summary>
        /// Метод проверяет валидность налого номера компании.
        /// </summary>
        /// <returns>true - если ИНН валидный. False - если инн не валидный</returns>
        bool Validate();
    }
}