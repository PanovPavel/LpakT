using LpakBL.Model.TaxNumberValidator;
using Xunit;

namespace LpakBL.Model.Exception.Tests
{
    public class CompanyInnValidatorTest
    {
        [Fact]
        public void ValidateTest()
        {
            // Act
            // Assert
            Assert.Equal(true, new CompanyInnValidator("0905786581").Validate());
            Assert.Equal(true, new CompanyInnValidator("1491763780").Validate());
            Assert.Equal(true, new CompanyInnValidator("7232281509").Validate());
            Assert.Equal(true, new CompanyInnValidator("5567796383").Validate());
            Assert.Equal(true, new CompanyInnValidator("4368477140").Validate());
            Assert.Equal(true, new CompanyInnValidator("8851775063").Validate());
            Assert.Equal(true, new CompanyInnValidator("3376037670").Validate());
            Assert.Equal(true, new CompanyInnValidator("0846324230").Validate());
        }

        [Fact]
        public void ValidateTestReturnsFalse()
        {
            Assert.Equal(false, new CompanyInnValidator("sadascxzczxcasdd").Validate());
            Assert.Equal(false, new CompanyInnValidator("sadqwedqsad").Validate());
            Assert.Equal(false, new CompanyInnValidator(" 7232281509").Validate());
            Assert.Equal(false, new CompanyInnValidator("5567796383 ").Validate());
            Assert.Equal(false, new CompanyInnValidator("43684 77140").Validate());
            Assert.Equal(false, new CompanyInnValidator("88517750x3").Validate());
            Assert.Equal(false, new CompanyInnValidator("0000000000").Validate());
            Assert.Equal(false, new CompanyInnValidator("21123").Validate());
            Assert.Equal(false, new CompanyInnValidator("").Validate());
            Assert.Equal(false, new CompanyInnValidator(null).Validate());
        }
    }
}