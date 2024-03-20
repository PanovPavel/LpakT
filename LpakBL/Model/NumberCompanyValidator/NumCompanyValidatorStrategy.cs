namespace LpakBL.Model.NumberCompanyValidator
{
    /// <summary>
    /// Валидатор налогового номера компании.
    /// </summary>
    public class NumCompanyValidatorStrategy
    {
        /// <summary>
        /// Тип организации номер который необходимо валидировать.
        /// </summary>
        private readonly TypeNumberOrganization _typeOrganization;
        /// <summary>
        /// Конструктор класса <see cref="NumCompanyValidatorStrategy"/>.
        /// </summary>
        /// <param name="typeOrganization">Тип налоговый организации номер которой необходимо валидирова</param>
        public NumCompanyValidatorStrategy(TypeNumberOrganization typeOrganization)
        {
            _typeOrganization = typeOrganization;
        }
        /// <summary>
        /// Возвращает валидатор налогового номера компании по в зависимости от выбранного типа организации
        /// </summary>
        /// <param name="valueTaxNumber">Значение налогового номера</param>
        /// <returns>Валидатор</returns>
        public InnValidator GetTypeValidator(string valueTaxNumber)
        {
            switch (_typeOrganization)
            {
                case TypeNumberOrganization.CompanyInn:
                    return new CompanyInnValidator(valueTaxNumber);
                case TypeNumberOrganization.IndividualInn:
                    return new IndividualInnValidator(valueTaxNumber);
                default:
                    return new OtherInnValidator(valueTaxNumber);
            }
        }
        
        
    }
}