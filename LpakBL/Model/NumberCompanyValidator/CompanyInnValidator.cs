using System.Linq;

namespace LpakBL.Model.NumberCompanyValidator
{
    public class CompanyInnValidator : InnValidator
    {
        private readonly string _taxNumber;
        public CompanyInnValidator(string taxNumber)
        {
            _taxNumber = taxNumber;
        }
        public bool Validate()
        {
            if(_taxNumber == null || _taxNumber.Length != 10) return false;
            if(_taxNumber.Count(c => c.ToString()=="0") == _taxNumber.Length) return false;
            int firstControlDigit = GetControlFigure(new[] { 2, 4, 10, 3, 5, 9, 4, 6, 8 });

            int.TryParse(_taxNumber[9].ToString(), out int ten);
            return firstControlDigit == ten;
        }
        private int GetControlFigure(int[] coefficientsMultiplier)
        {
            if(coefficientsMultiplier == null || coefficientsMultiplier.Length != 9) return -1;
            var controlFigure = 0;
            for (var i = 0; i < _taxNumber.Length-1; i++)
            {
                if(!int.TryParse(_taxNumber[i].ToString(), out int count)) return -1;
                controlFigure += count * coefficientsMultiplier[i];
            }
            controlFigure = controlFigure % 11;
            while (controlFigure>=10)
            {
                controlFigure = controlFigure % 10;
            }
            return controlFigure;
        }
        
    }
}