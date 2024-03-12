using LpakBL.Model.NumberCompanyValidator;
using Xunit;

namespace LpakBLTests.Model.TaxNumberValidator
{
    public class CompanyInnValidatorTest
    {
        [Theory]
        [InlineData("0905786581")]
        [InlineData("1491763780")]
        [InlineData("7232281509")]
        [InlineData("5567796383")]
        [InlineData("4368477140")]
        [InlineData("8851775063")]
        [InlineData("3376037670")]
        [InlineData("0846324230")]

        public void ValidateTest(string taxNumber)
        {
            // Act
            // Assert
            Assert.Equal(true, new CompanyInnValidator(taxNumber).Validate());

        }
        [Theory]
        [InlineData("sadascxzczxcasdd")]
        [InlineData("sadqwedqsad")]
        [InlineData(" 7232281509")]
        [InlineData("5567796383 ")]
        [InlineData("43684 77140")]
        [InlineData("88517750x3")]
        [InlineData("0000000000")]
        [InlineData("21123")]
        [InlineData("")]
        [InlineData(null)]
        public void ValidateTestReturnsFalse(string taxNumber)
        {
            Assert.Equal(false, new CompanyInnValidator(taxNumber).Validate());

        }
    }
}