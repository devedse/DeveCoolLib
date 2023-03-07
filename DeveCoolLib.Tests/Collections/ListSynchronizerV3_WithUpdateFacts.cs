using DeveCoolLib.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace DeveCoolLib.Tests.Collections
{
    public class ListSynchronizerV3_WithUpdateFacts
    {
        private readonly ITestOutputHelper _output;
        private readonly string _currentTestName;

        public ListSynchronizerV3_WithUpdateFacts(ITestOutputHelper output)
        {
            _output = output;
            var type = output.GetType();
            var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            var test = (ITest)testMember.GetValue(output);
            _currentTestName = test.DisplayName;
        }

        [Fact]
        public void RemovesItems()
        {
            //Arrange
            var data = new List<ComparableObject2>()
            {
                new(1, "Devedse", "Bush", 1),
                new(2, "Superman", "Bandi", 4),
            };

            var desiredData = new ObservableCollection<ComparableObject2>()
            {
                new(1, "Devedse", "Bush", 1)
            };

            SynchronizeAndVerify(data, desiredData);
        }

        [Fact]
        public void AddsItems()
        {
            //Arrange
            var data = new List<ComparableObject2>()
            {
                new(1, "Devedse", "Bush", 1),
                new(2, "Superman", "Bandi", 4),
            };

            var desiredData = new ObservableCollection<ComparableObject2>()
            {
                new(1, "Devedse", "Bush", 1),
                new(2, "Superman", "Bandi", 4),
                new(3, "Coolguy", "Blah", 12)
            };

            SynchronizeAndVerify(data, desiredData);
        }

        [Fact]
        public void AddsAndRemovesItems()
        {
            //Arrange
            var data = new List<ComparableObject2>()
            {
                new(2, "Superman", "Bandi", 4),
                new(3, "Coolguy", "Blah", 12),
                new(4, "Gunther", "Schneider", 55)
            };

            var desiredData = new ObservableCollection<ComparableObject2>()
            {
                new(1, "Devedse", "Bush", 1),
                new(2, "Superman", "Bandi", 4),
                new(3, "Coolguy", "Blah", 12)
            };

            SynchronizeAndVerify(data, desiredData);
        }

        [Fact]
        public void UpdatesItems()
        {
            //Arrange
            var data = new List<ComparableObject2>()
            {
                new(1, "Devedse", "Bush", 1),
                new(2, "Superman", "Bandi", 4),
                new(3, "Coolguy", "Blah", 12)
            };

            var desiredData = new ObservableCollection<ComparableObject2>()
            {
                new(1, "Devedse", "Bush", 1),
                new(2, "Superman2", "Bandi2", 42),
                new(3, "Coolguy", "Blah", 12)
            };

            SynchronizeAndVerify(data, desiredData);
        }

        [Fact]
        public void AddsAndRemovesAndUpdatesItems()
        {
            //Arrange
            var data = new List<ComparableObject2>()
            {
                new(2, "Superman", "Bandi", 4),
                new(3, "Coolguy", "Blah", 12),
                new(4, "Gunther", "Schneider", 55)
            };

            var desiredData = new ObservableCollection<ComparableObject2>()
            {
                new(1, "Devedse", "Bush", 1),
                new(2, "Superman", "Bandi", 4),
                new(3, "Coolguy2", "Blah2", 122)
            };

            SynchronizeAndVerify(data, desiredData);
        }

        private void SynchronizeAndVerify(List<ComparableObject2> data, ObservableCollection<ComparableObject2> desiredData)
        {
            //Act
            var expectedCount = desiredData.Count;

            desiredData.CollectionChanged += (sender, e) => _output.WriteLine($"{_currentTestName}: {e.Action}");

            ListSynchronizerV3_WithUpdate.SynchronizeLists(data, desiredData, t => t.Key, (oldItem, newItem) =>
            {
                oldItem.FirstName = newItem.FirstName;
                oldItem.LastName = newItem.LastName;
                oldItem.Length = newItem.Length;
            });

            //Assert
            Assert.Equal(expectedCount, data.Count);
            Assert.Equal(data.Count, desiredData.Count);

            var sortedData = data.OrderBy(t => t.Key).ToList();
            var sortedDesiredData = desiredData.OrderBy(t => t.Key).ToList();
            for (int i = 0; i < sortedData.Count; i++)
            {
                Assert.Equal(sortedData[i].Key, sortedDesiredData[i].Key);
                Assert.Equal(sortedData[i].FirstName, sortedDesiredData[i].FirstName);
                Assert.Equal(sortedData[i].LastName, sortedDesiredData[i].LastName);
                Assert.Equal(sortedData[i].Length, sortedDesiredData[i].Length);
            }
        }
    }
}
