using DeveCoolLib.TextFormatting;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DeveCoolLib.Tests.TextFormatting
{
    public class TableToTextPrinterFacts
    {
        [Fact]
        public void ReturnsTheRightString()
        {
            //Arrange
            var data = new List<List<string>>();
            data.Add(new List<string>() { "", "First Name", "Last Name" });
            data.Add(null);
            data.Add(new List<string>() { "1", "Heinz", "Dovenschmirtz" });
            data.Add(new List<string>() { "2", "Mickey", "Mouse" });

            //Act
            var result = TableToTextPrinter.TableToText(data);

            //Assert
            var expected = $@"|   | First Name |   Last Name   |{Environment.NewLine}----------------------------------{Environment.NewLine}| 1 |   Heinz    | Dovenschmirtz |{Environment.NewLine}| 2 |   Mickey   |     Mouse     |{Environment.NewLine}";
            Assert.Equal(expected, result);
        }
    }
}
