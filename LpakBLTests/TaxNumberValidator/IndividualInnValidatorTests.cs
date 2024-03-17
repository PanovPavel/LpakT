using LpakBL.Model.NumberCompanyValidator;
using Xunit;

namespace LpakBLTests.TaxNumberValidator
{
    
    public class IndividualInnValidatorTests
    {
        [Theory]
        [InlineData("609153377455")]
        [InlineData("743532004084")]
        [InlineData("612397515424")]
        [InlineData("559046960541")]
        [InlineData("929933670984")]
        [InlineData("294868989407")]
        [InlineData("190411846248")]
        [InlineData("073132709851")]
        [InlineData("929933670984")]
        public void ValidateTest(string taxNumber)
        {
            // Act
            // Assert
            Assert.Equal(true, new IndividualInnValidator(taxNumber).Validate());
        }

        [Theory]
        [InlineData("000000000000")]
        [InlineData("609153231377455")]
        [InlineData("7435320024084")]
        [InlineData("61239751542x")]
        [InlineData("5590469605y1")]
        [InlineData("92993367091233321384")]
        [InlineData("2")]
        [InlineData("073132709851 ")]
        [InlineData("")]
        [InlineData("07313 270981")]
        [InlineData("sadddddddddd")]
        [InlineData("aaaaaaaaaaaaddd")]
        [InlineData(null)]

        public void ValidateTestReturnsFalse(string taxNumber)
        {
            Assert.Equal(false, new IndividualInnValidator(taxNumber).Validate());
        }
    }
}