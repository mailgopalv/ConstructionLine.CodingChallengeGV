using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
        }

        /// <summary>
        ///     Search operation to identify and return filtered color and size
        /// </summary>
        /// <param name="options">
        ///     (1) Options.SizeList to enter multiple sizes to include range of sizes
        ///     (2) Options.ColorList to enter multiple colors to include range of colors
        /// </param>
        /// <returns>
        ///     Search result with list of shirts, colors count list and size count list
        /// </returns>
        public SearchResults Search(SearchOptions options)
        {
            if (_shirts == null)
                throw new InvalidOperationException("Shirts datastore not available to perform search");

            if (options == null || options.Colors == null || options.Sizes == null)
                throw new ArgumentNullException("Search options not available to perform search");
            
            if (!options.Sizes.Any() && !options.Colors.Any())
                throw new ArgumentException("Invalid search options - Both size and color search options cannot be empty");

            try
            {
                var shirts = _shirts.Where(x => (!options.Colors.Any() || options.Colors.Select(y => y.Id).Contains(x.Color.Id))
                                    && (!options.Sizes.Any() || options.Sizes.Select(z => z.Id).Contains(x.Size.Id))).ToList();
                var colorCounts = UpdateColorCount(shirts);
                var sizeCounts = UpdateSizeCount(shirts);

                return new SearchResults { Shirts = shirts, ColorCounts = colorCounts, SizeCounts = sizeCounts };
            }
            catch(Exception ex)
            {
                throw new Exception("Exception occured during search operation", ex);
            }
        }

        private List<ColorCount> UpdateColorCount(List<Shirt> resultShirts)
        {
            List<ColorCount> resultColorCounts = new List<ColorCount>();
            Color.All.ForEach((item) =>
            {
                var currentColorCount = new ColorCount
                {
                    Color = item,
                    Count = resultShirts.Count(x => x.Color.Id == item.Id)
                };

                resultColorCounts.Add(currentColorCount);
            });
            return resultColorCounts;
        }

        private List<SizeCount> UpdateSizeCount(List<Shirt> resultShirts)
        {
            List<SizeCount> resultSizeCounts = new List<SizeCount>();
            Size.All.ForEach((item) =>
            {
                var currentSizeCount = new SizeCount
                {
                    Size = item,
                    Count = resultShirts.Count(x => x.Size.Id == item.Id)
                };

                resultSizeCounts.Add(currentSizeCount);
            });
            return resultSizeCounts;
        }
    }
}