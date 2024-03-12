using System.Linq;

namespace LpakBL.Model.TaxNumberValidator
{
    public class IndividualInnValidator : InnValidator
    {
        private readonly string _taxNumber;
        public IndividualInnValidator(string taxNumber)
        {
            _taxNumber = taxNumber;
        } 
        public bool Validate()
        {
            if(_taxNumber == null || _taxNumber.Length != 12) return false;
            if(_taxNumber.Count(c => c.ToString()=="0") == _taxNumber.Length) return false; 
            int firstControlDigit = GetControlFigure(new[] { 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 });
            int secondControlDigit = GetControlFigure(new[] {3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8});
            int.TryParse(_taxNumber[10].ToString(), out int ten);
            int.TryParse(_taxNumber[11].ToString(), out int eleven);
            if(firstControlDigit == ten && secondControlDigit == eleven) return true;
            return false;
        }

        private int GetControlFigure(int[] coefficientsMultiplier)
        {
            if(
               coefficientsMultiplier == null
               || coefficientsMultiplier.Length > 11
               || coefficientsMultiplier.Length < 10
               ) return -1;
            int controlFigure = 0;
            for (int i = 0; i < (coefficientsMultiplier.Length == 10 ? _taxNumber.Length-2 : _taxNumber.Length-1); i++)
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