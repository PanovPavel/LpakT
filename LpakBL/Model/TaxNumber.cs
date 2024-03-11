using System;
using LpakBL.Model;
using LpakBL.Model.Exception;

namespace LpakBL
{
    public class TaxNumber
    {
        public EnumTypeOrganization EnumTypeOrganization{ get; set; }
        public string ValueTaxNumber { get; set; }

        public TaxNumber(EnumTypeOrganization enumTypeOrganization, string valueTaxNumber)
        {
            EnumTypeOrganization = enumTypeOrganization;
            ValueTaxNumber = valueTaxNumber;
        }
        public ITaxNumValidator GetValidator()
        {
            switch (EnumTypeOrganization)
            {
                case EnumTypeOrganization.LegalCompany:
                    return new LegalCompanyTaxNumValidator();
                case EnumTypeOrganization.IndividualBusiness:
                    return new IndividualTaxNumValidator();
                case EnumTypeOrganization.OtherOrganization:
                    return new OtherTaxNumValidator();
                default:
                    throw new NotImplementedException($"Validator for is not implemented.");
            }
        }
    }
}