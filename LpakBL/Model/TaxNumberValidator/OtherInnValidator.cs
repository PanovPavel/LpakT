namespace LpakBL.Model.TaxNumberValidator
{
    public class OtherInnValidator:InnValidator
    {
        private readonly string _taxNumber;
        public OtherInnValidator(string taxNumber)
        {
            _taxNumber = taxNumber;
        } 
        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(_taxNumber);
        }
    }
}