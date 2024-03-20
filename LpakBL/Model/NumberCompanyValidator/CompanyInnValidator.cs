using System.Linq;

namespace LpakBL.Model.NumberCompanyValidator
{
    /// <summary>
    /// Этот класс используется для валидации налого номера компании.
    /// </summary>
    public class CompanyInnValidator : InnValidator
    {
        private readonly string _taxNumber;
        /// <summary>
        /// Конструктор класса <see cref="CompanyInnValidator"/>.
        /// </summary>
        /// <param name="taxNumber">налоговый номер компании, из 10 цифр</param>
        public CompanyInnValidator(string taxNumber)
        {
            _taxNumber = taxNumber;
        }
        
        /// <summary>
        /// Проверка валидности налого номера компании.
        /// </summary>
        /// <returns>true - если номер валидный, false - если номер не явлется валидный</returns>
        public bool Validate()
        {
            if(_taxNumber == null || _taxNumber.Length != 10) return false;
            if(_taxNumber.Count(c => c.ToString()=="0") == _taxNumber.Length) return false;
            int firstControlDigit = GetControlFigure(new[] { 2, 4, 10, 3, 5, 9, 4, 6, 8 }, _taxNumber);

            int.TryParse(_taxNumber[9].ToString(), out int ten);
            return firstControlDigit == ten;
        }

        /// <summary>
        /// Возвращает номер контрольной цифры налогово номера, в зависимости от полученного массива Multiplier цифр, 
        /// </summary>
        /// <param name="coefficientsMultiplier">Multiplier массив чисел для формирования контрольго числа</param>
        /// <param name="taxNumber">Налоговый номер</param>
        /// <returns>Контрольное число</returns>
        private int GetControlFigure(int[] coefficientsMultiplier, string taxNumber)
        {
            if(coefficientsMultiplier == null || coefficientsMultiplier.Length != 9) return -1;
            var controlFigure = 0;
            for (var i = 0; i < taxNumber.Length-1; i++)
            {
                if(!int.TryParse(taxNumber[i].ToString(), out int count)) return -1;
                controlFigure += count * coefficientsMultiplier[i];
            }
            controlFigure %= 11;
            while (controlFigure>=10)
            {
                controlFigure %= 10;
            }
            return controlFigure;
        }
        
    }
}