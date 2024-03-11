using System.Text.RegularExpressions;

namespace LpakBL.Model.Exception
{
    public class IndividualTaxNumValidator : ITaxNumValidator
    {
        public bool Validate(string taxNumber)
        {
            return !string.IsNullOrWhiteSpace(taxNumber) && Regex.IsMatch(taxNumber, "^[0-9]{10}$");
        }
    }
}