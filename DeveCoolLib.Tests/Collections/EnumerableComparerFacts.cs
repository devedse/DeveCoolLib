using DeveCoolLib.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DeveCoolLib.Tests.Collections
{
    public class EnumerableComparerFacts
    {
        public class SequenceEqualEqualityComparer
        {
            [Fact]
            public void DetectsEqualSimple()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, (first, second) => first.FirstName == second.FirstName && first.LastName == second.LastName);

                //Assert
                Assert.True(result);
            }

            [Fact]
            public void DetectsEqualSimpleWithCaseDifference()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "user"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "test", LastName = "User"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, (first, second) => first.FirstName.Equals(second.FirstName, StringComparison.OrdinalIgnoreCase) && first.LastName.Equals(second.LastName, StringComparison.OrdinalIgnoreCase));

                //Assert
                Assert.True(result);
            }

            [Fact]
            public void DetectsNotEqualSimple()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "user"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "test", LastName = "UserZ"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, (first, second) => first.FirstName.Equals(second.FirstName, StringComparison.OrdinalIgnoreCase) && first.LastName.Equals(second.LastName, StringComparison.OrdinalIgnoreCase));

                //Assert
                Assert.False(result);
            }

            [Fact]
            public void DetectsEqualMultipleInFirst()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"},
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, (first, second) => first.FirstName == second.FirstName && first.LastName == second.LastName);

                //Assert
                Assert.False(result);
            }

            [Fact]
            public void DetectsEqualMultipleInSecond()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"},
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, (first, second) => first.FirstName == second.FirstName && first.LastName == second.LastName);

                //Assert
                Assert.False(result);
            }

            [Fact]
            public void DetectsEqualMultipleInBoth()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"},
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"},
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, (first, second) => first.FirstName == second.FirstName && first.LastName == second.LastName);

                //Assert
                Assert.True(result);
            }

            [Fact]
            public void DetectsNotEqualMultipleIfOrderedIncorrectly()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test1", LastName = "User1"},
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"},
                    new ComparableObject() { FirstName = "Test1", LastName = "User1"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, (first, second) => first.FirstName == second.FirstName && first.LastName == second.LastName);

                //Assert
                Assert.False(result);
            }
        }


        public class SequenceEqualProperty
        {
            [Fact]
            public void DetectsEqualSimple()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, t => t.FirstName, t => t.LastName);

                //Assert
                Assert.True(result);
            }

            [Fact]
            public void DetectsNotEqualSimpleWithCaseDifference()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "user"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "test", LastName = "User"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, t => t.FirstName, t => t.LastName);

                //Assert
                Assert.False(result);
            }

            [Fact]
            public void DetectsNotEqualSimple()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "user"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "test", LastName = "UserZ"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, t => t.FirstName, t => t.LastName);

                //Assert
                Assert.False(result);
            }

            [Fact]
            public void DetectsEqualMultipleInFirst()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"},
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, t => t.FirstName, t => t.LastName);

                //Assert
                Assert.False(result);
            }

            [Fact]
            public void DetectsEqualMultipleInSecond()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"},
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, t => t.FirstName, t => t.LastName);

                //Assert
                Assert.False(result);
            }

            [Fact]
            public void DetectsEqualMultipleInBoth()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"},
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"},
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, t => t.FirstName, t => t.LastName);

                //Assert
                Assert.True(result);
            }

            [Fact]
            public void DetectsNotEqualMultipleIfOrderedIncorrectly()
            {
                //Arrange
                var first = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test1", LastName = "User1"},
                    new ComparableObject() { FirstName = "Test", LastName = "User"}
                };
                var second = new List<ComparableObject>()
                {
                    new ComparableObject() { FirstName = "Test", LastName = "User"},
                    new ComparableObject() { FirstName = "Test1", LastName = "User1"}
                };

                //Act
                var result = EnumerableComparer.SequenceEqual(first, second, t => t.FirstName, t => t.LastName);

                //Assert
                Assert.False(result);
            }
        }
    }
}
