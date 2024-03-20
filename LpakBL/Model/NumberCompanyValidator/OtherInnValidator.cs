namespace LpakBL.Model.NumberCompanyValidator
{
    /// <summary>
    /// Валидатор для других типов компаний
    /// </summary>
    public class OtherInnValidator:InnValidator
    {
        /// <summary>
        /// Налоговый номер компании
        /// </summary>
        private readonly string _taxNumber;
        /// <summary>
        /// Конструктор класса OtherInnValidator
        /// </summary>
        /// <param name="taxNumber">Налоговый номер</param>
        public OtherInnValidator(string taxNumber)
        {
            _taxNumber = taxNumber;
        } 
        
        /// <summary>
        /// Валидация налогового номера. 
        /// </summary>
        /// <returns>Возвращает true, если налоговый номер не null or withSpace. False - если значение пустое NullOrSpace</returns>
        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(_taxNumber);
        }
    }
}