using System.Text.RegularExpressions;

namespace LpakBL.Model
{
    public class LegalCompanyTaxNumValidator : ITaxNumValidator
    {
        public bool Validate(string taxNumber)
        {
            return !string.IsNullOrWhiteSpace(taxNumber) && Regex.IsMatch(taxNumber, "^[0-9]{14}$");
        }
    }
}