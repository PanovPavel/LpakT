namespace LpakBL.Model.TaxNumberValidator
{
    public class NumCompanyValidatorStrategy
    {
        private readonly TypeNumberOrganization _typeOrganization;
        public NumCompanyValidatorStrategy(TypeNumberOrganization typeOrganization)
        {
            _typeOrganization = typeOrganization;
        }
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