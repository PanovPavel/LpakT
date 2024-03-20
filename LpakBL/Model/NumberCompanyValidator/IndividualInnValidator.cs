using System.Linq;

namespace LpakBL.Model.NumberCompanyValidator
{
    /// <summary>
    /// Валидцация ИНН для индивидуальных предпринимателей
    /// </summary>
    public class IndividualInnValidator : InnValidator
    {
        /// <summary>
        /// Налоговый номер
        /// </summary>
        private readonly string _taxNumber;
        /// <summary>
        /// Конструктор класса <see cref="IndividualInnValidator"/>
        /// </summary>
        /// <param name="taxNumber">налоговый номер</param>
        public IndividualInnValidator(string taxNumber)
        {
            _taxNumber = taxNumber;
        } 
        
        /// <summary>
        /// Проверка валидности налого номера компании.
        /// </summary>
        /// <returns>true - если номер валидный, false - если номер не явлется валидный</returns>
        public bool Validate()
        {
            if(_taxNumber == null || _taxNumber.Length != 12) return false;
            if(_taxNumber.Count(c => c.ToString()=="0") == _taxNumber.Length) return false; 
            int firstControlDigit = GetControlFigure(new[] { 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 }, _taxNumber);
            int secondControlDigit = GetControlFigure(new[] {3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8}, _taxNumber);
            int.TryParse(_taxNumber[10].ToString(), out int ten);
            int.TryParse(_taxNumber[11].ToString(), out int eleven);
            if(firstControlDigit == ten && secondControlDigit == eleven) return true;
            return false;
        }

        /// <summary>
        /// Возвращает номер контрольной цифры налогово номера, в зависимости от полученного массива Multiplier цифр, 
        /// </summary>
        /// <param name="coefficientsMultiplier">Multiplier массив чисел для формирования контрольго числа</param>
        /// <param name="taxNumber"></param>
        /// <returns>Налоговый номер</returns>
        private int GetControlFigure(int[] coefficientsMultiplier, string taxNumber)
        {
            if(
               coefficientsMultiplier == null
               || coefficientsMultiplier.Length > 11
               || coefficientsMultiplier.Length < 10
               ) return -1;
            int controlFigure = 0;
            for (int i = 0; i < (coefficientsMultiplier.Length == 10 ? taxNumber.Length-2 : taxNumber.Length-1); i++)
            {
                if(!int.TryParse(taxNumber[i].ToString(), out int count)) return -1;
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