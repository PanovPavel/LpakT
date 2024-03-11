namespace LpakBL.Model
{
    public class OtherTaxNumValidator:ITaxNumValidator
    {
        public bool Validate(string taxNumber)
        {
            return !string.IsNullOrWhiteSpace(taxNumber);
        }
    }
}