using System;
using LpakBL;
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
        [InlineData("David")]
        [InlineData("Emi ly     ddsadc")]
        [InlineData("Alex")]
        [InlineData("Olivia")]
        public void Customer_Constructor_PropertyNameTestCorrect(string name)
        {
            var taxNumber = "032451591077";
            var comment = "This is a comment";
            Assert.Equal(name, new Customer(name, taxNumber, comment).Name);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("   ")]
        public void Customer_Constructor_PropertyNameTestWrong(string name)
        {
            var taxNumber = "032451591077";
            var comment = "This is a comment";
            Assert.Throws<IncorrectLongOrNullException>(()=>new Customer(name, taxNumber, comment).Name);
        }
        
        
        
        
        
        
    }
}