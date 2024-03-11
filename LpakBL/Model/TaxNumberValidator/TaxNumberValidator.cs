using System;
using LpakBL.Model.Exception;

namespace LpakBL.Model
{
    //TODO: Активировать патерн стратегия в зависимости от длинны ИНН, а не от Enum
    //#region MyRegion
    /*
     https://info.gosuslugi.ru/articles/Валидация/
     * 8. Валидация ИНН ИП и ФЛ – в ИНН ИП и ФЛ 11 и 12 цифры являются контрольным числом. Алгоритм проверки контрольного числа следующий. Вычислить 1-ю контрольную цифру: Вычислить сумму произведений цифр ИНН (с 1-й по 10-ю) на следующие коэффициенты:
7, 2, 4, 10, 3, 5, 9, 4, 6, 8 (т.е. 7 * ИНН[1] + 2 * ИНН[2] + ...) Вычислить младший разряд остатка от деления полученной суммы на 11. Вычислить 2-ю контрольную цифру: Вычислить сумму произведений цифр ИНН (с 1-й по 11-ю) на следующие коэффициенты:
3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 (т.е. 3 * ИНН[1] + 7 * ИНН[2] + ...) Вычислить младший разряд остатка от деления полученной суммы на 11. Сравнить 1-ю контрольную цифру с 11-й цифрой ИНН и сравнить 2-ю контрольную цифру с 12-й цифрой ИНН. Если они равны, то ИНН верный.

9. Валидация ИНН организации – в ИНН организации 10 цифра является контрольным разрядом. Алгоритм проверки контрольного числа следующий. Вычислить сумму произведений цифр ИНН (с 1-й по 9-ю) на следующие коэффициенты — 2, 4, 10, 3, 5, 9, 4, 6, 8 (т.е. 2 * ИНН[1] + 4 * ИНН[2] + ...). Вычислить остаток от деления полученной суммы на 11. Сравнить младший разряд полученного остатка от деления с младшим разрядом ИНН. Если они равны, то ИНН верный.
     */
    //#endregion


    public static class TaxNumberValidator
    {
        private enum EnumTypeOrganization : int
        {
            IndividualInn = 12,
            CompanyInn = 14
        }
        public static InnValidator GetTypeValidator(string valueTaxNumber)
        {
            switch (valueTaxNumber.Length)
            {
                case (int)EnumTypeOrganization.CompanyInn:
                    return new CompanyInnValidator(valueTaxNumber);
                case (int)EnumTypeOrganization.IndividualInn:
                    return new IndividualInnValidator(valueTaxNumber);
                default:
                    return new OtherInnValidator(valueTaxNumber);
                //TODO: Исключение о неверном формате ИНН
                //TODO: Написать Enum для длинны ИНН 12, 14
            }
        }
        
        
    }
}