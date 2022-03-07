using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        [Test]
        public void Test()
        {
            var shirts = LoadShirtsToStore();

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Red},
                Sizes = new List<Size> {Size.Small}
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void SearchWithDatastoreAsNullReturnInvalidOperationException()
        {
            List<Shirt> shirts = null;
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Blue },
                Sizes = new List<Size> { Size.Small }
            };

            Assert.Throws<InvalidOperationException>(() => searchEngine.Search(searchOptions), 
                "Shirts datastore not available to perform search");
        }

        [Test]
        public void SearchWithOptionAsNullReturnArgumentNullException()
        {
            List<Shirt> shirts = LoadShirtsToStore();
            var searchEngine = new SearchEngine(shirts);
            SearchOptions searchOptions = null;

            Assert.Throws<ArgumentNullException>(() => searchEngine.Search(searchOptions),
                "Search options not available to perform search");
        }

        [Test]
        public void SearchWithOptionColorAsNullReturnArgumentNullException()
        {
            List<Shirt> shirts = LoadShirtsToStore();
            var searchEngine = new SearchEngine(shirts);
            SearchOptions searchOptions = new SearchOptions
            {
                Colors = null, Sizes = new List<Size> { Size.Small }
            };

            Assert.Throws<ArgumentNullException>(() => searchEngine.Search(searchOptions),
                "Search options not available to perform search");
        }

        [Test]
        public void SearchWithOptionSizeAsNullReturnArgumentNullException()
        {
            List<Shirt> shirts = LoadShirtsToStore();
            var searchEngine = new SearchEngine(shirts);
            SearchOptions searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Black },
                Sizes = null
            };

            Assert.Throws<ArgumentNullException>(() => searchEngine.Search(searchOptions),
                "Search options not available to perform search");
        }

        [Test]
        public void SearchWithOptionContainingZeroItemsReturnArgumentException()
        {
            List<Shirt> shirts = LoadShirtsToStore();
            var searchEngine = new SearchEngine(shirts);
            SearchOptions searchOptions = new SearchOptions
            {
                Colors = new List<Color>(),
                Sizes = new List<Size>()
            };

            Assert.Throws<ArgumentException>(() => searchEngine.Search(searchOptions),
                "Invalid search options - Both size and color search options cannot be empty");
        }

        [Test]
        public void SearchWithOptions2Sizes1ColorReturnValidResult()
        {
            var shirts = LoadShirtsToStore();

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Red },
                Sizes = new List<Size> { Size.Small, Size.Medium }
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);

            // Two shirts match search criteria
            Assert.AreEqual(2, results.Shirts.Count);

            // Color counts Red should return 2 but other colour counts return 0 
            var validateColor = results.ColorCounts.Where(x => x.Count != 0).ToList();
            Assert.AreEqual(1, validateColor.Count());
            var identifiedColor = validateColor.FirstOrDefault();
            Assert.AreEqual(Color.Red, identifiedColor.Color);
            Assert.AreEqual(2, identifiedColor.Count);

            // Size counts Small and medium should return 1 each but other sizes counts return 0
            Assert.AreEqual(2, results.SizeCounts.Count(x => x.Count != 0));
        }


        [Test]
        public void SearchWithOptions1Size2ColorsReturnValidResult()
        {
            var shirts = LoadShirtsToStore();

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Red, Color.Black },
                Sizes = new List<Size> { Size.Small }
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);

            // Two shirts match search criteria
            Assert.AreEqual(2, results.Shirts.Count);
            
            // Size counts Small should return 2 but other sizes counts return 0
            var validateSize = results.SizeCounts.Where(x => x.Count != 0).ToList();
            Assert.AreEqual(1, validateSize.Count());
            var identifiedSize = validateSize.FirstOrDefault();
            Assert.AreEqual(Size.Small, identifiedSize.Size);
            Assert.AreEqual(2, identifiedSize.Count);

            // Color counts Red and Black should return 1 but other colour counts return 0 
            Assert.AreEqual(2, results.ColorCounts.Count(x => x.Count != 0));
        }

        [Test]
        public void SearchWithOptions2SizesOnlyReturnValidResult()
        {
            var shirts = LoadShirtsToStore();

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color>(),
                Sizes = new List<Size> { Size.Small, Size.Medium }
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
            Assert.AreEqual(10, results.Shirts.Count);
            Assert.AreEqual(2, results.SizeCounts.Count(x => x.Count != 0));
            Assert.AreEqual(5, results.ColorCounts.Count(x => x.Count != 0));
        }

        [Test]
        public void SearchWithOptions2ColorsOnlyReturnValidResult()
        {
            var shirts = LoadShirtsToStore();

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Blue, Color.Red },
                Sizes = new List<Size>()
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
            Assert.AreEqual(6, results.Shirts.Count);
            Assert.AreEqual(3, results.SizeCounts.Count(x => x.Count != 0));
            Assert.AreEqual(2, results.ColorCounts.Count(x => x.Count != 0));
        }

        [Test]
        public void SearchWithInvalidDatastoreReturnException()
        {
            List<Shirt> shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Blue - Small", Size.Small, Color.Blue),
                 new Shirt(Guid.NewGuid(), "Blue - Small", null, null)
            };

            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Red },
                Sizes = new List<Size> { Size.Small, Size.Medium }
            };

            Assert.Throws<Exception>(() => searchEngine.Search(searchOptions),
                "Exception occured during search operation");


        }

        private List<Shirt> LoadShirtsToStore()
        {
            return new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Blue - Small", Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), "Black - Small", Size.Small, Color.Black),
                new Shirt(Guid.NewGuid(), "White - Small", Size.Small, Color.White),
                new Shirt(Guid.NewGuid(), "Yellow - Small", Size.Small, Color.Yellow),

                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red),
                new Shirt(Guid.NewGuid(), "Blue - Medium", Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "White - Medium", Size.Medium, Color.White),
                new Shirt(Guid.NewGuid(), "Yellow - Medium", Size.Medium, Color.Yellow),

                new Shirt(Guid.NewGuid(), "Red - Large", Size.Large, Color.Red),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Black - Large", Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), "White - Large", Size.Large, Color.White),
                new Shirt(Guid.NewGuid(), "Yellow - Large", Size.Large, Color.Yellow),

            };
        }
    }
}
