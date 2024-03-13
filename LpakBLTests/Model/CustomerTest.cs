using System;
using LpakBL.Model;
using LpakBL.Model.Exception;
using Xunit;

namespace LpakBLTests.Model
{
    public class CustomerTest
    {
        [Theory]
        [InlineData("Jo5414194215322566525560hn")]
        [InlineData("Jan jbhy h hui oi ho e")]
        [InlineData("   Michael")]
        [InlineData("Alice")]
        [InlineData("Bob")]
        [InlineData("Mary")]
        [InlineData("ЫФвыфввыфВыф")]
        [InlineData("Emi ly     ddsadc")]
        [InlineData("Alex")]
        [InlineData("Olivia")]
        public void Customer_Constructor_CorrectNameTest(string name)
        {
            var taxNumber = "032451591077";
            var comment = "This is a comment";
            Assert.Equal(name, new Customer(name, taxNumber, comment, new FieldOfBusiness("sadsad")).Name);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("   ")]
        public void Customer_Constructor_WrongNameTest(string name)
        {
            var taxNumber = "032451591077";
            var comment = "This is a comment";
            Assert.Throws<IncorrectLongOrNullException>(()=>new Customer(name, taxNumber, comment, new FieldOfBusiness("sadsad")).Name);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("   ")]
        [InlineData(" sasad sad ")]
        [InlineData(" ыфвыф вйцВыф вйц ыфв6498 32 21у")]
        [InlineData("   ")]
        public void Customer_Constructor_CorrectCommentTest(string comment)
        {
            var taxNumber = "032451591077";
            var name = " Ы фывФВ ";
            Assert.Equal(comment, new Customer(name, taxNumber, comment, new FieldOfBusiness("sadsad")).Comment);

        }
        
        [Theory]
        [InlineData(null)]

        public void Customer_Constructor_WrongCommentTest(string comment)
        {
            var taxNumber = "032451591077";
            var name = " Ы фывФВ ";
            Assert.Throws<ArgumentNullException>(()=>new Customer(name, taxNumber, comment, new FieldOfBusiness("sadsad")));
        }
        
        
        [Theory]
        [InlineData("0905786581")]
        [InlineData("1491763780")]
        [InlineData("7232281509")]
        [InlineData("5567796383")]
        [InlineData("4368477140")]
        [InlineData("8851775063")]
        [InlineData("3376037670")]
        [InlineData("0846324230")]
        [InlineData("609153377455")]
        [InlineData("743532004084")]
        [InlineData("612397515424")]
        [InlineData("559046960541")]
        [InlineData("929933670984")]
        [InlineData("294868989407")]
        [InlineData("190411846248")]
        [InlineData("073132709851")]
        public void Customer_Constructor_CorrectTaxNumberTest(string taxNumber)
        {
            var name = " Ы фывФВ ";
            Assert.Equal(taxNumber, new Customer(name, taxNumber, "This is a comment", new FieldOfBusiness("sadsad")).TaxNumber);
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
        public void Customer_Constructor_WrongTaxNumberTest(string taxNumber)
        {
            var name = " Ы фывФВ ";
            Assert.Throws<InvalidTaxNumber>(()=>new Customer(name, taxNumber, new FieldOfBusiness("sadsad")));
        }
    }
}