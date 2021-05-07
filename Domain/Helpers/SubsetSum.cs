using System.Collections.Generic;
using System.Linq;

namespace Domain.Helpers
{
    public class SubsetSum
    {
        public static IEnumerable<IEnumerable<ConnectorModel>> Solve(IEnumerable<ConnectorModel> input, int targetMaxCurrentInAmps)
        {
            var result = new List<IEnumerable<ConnectorModel>>();
            int? shortestSubset = null;

            CalculateSubSets(input, targetMaxCurrentInAmps, ref shortestSubset, partialSubset: Enumerable.Empty<ConnectorModel>(), ref result);

            if (shortestSubset.HasValue)
            {
                result = result.Where(x => x.Count() == shortestSubset.Value)
                                .ToList();
            }

            return result;
        }

        private static void CalculateSubSets(IEnumerable<ConnectorModel> input, int targetSum, ref int? shortestSubSet, IEnumerable<ConnectorModel> partialSubset, ref List<IEnumerable<ConnectorModel>> result)
        {
            if (shortestSubSet.HasValue && partialSubset.Count() > shortestSubSet.Value)
            {
                return;
            }

            var sum = partialSubset.Sum(x => x.MaxCurrentInAmps);
            if (sum == targetSum)
            {
                result.Add(partialSubset);

                shortestSubSet = partialSubset.Count();
            }

            if (sum >= targetSum)
            {
                return;
            }

            int index = 0;
            foreach (var item in input)
            {
                var remainingItems = input.Skip(index + 1);

                var nextSubset = new List<ConnectorModel>(partialSubset) { item };

                CalculateSubSets(remainingItems, targetSum, ref shortestSubSet, nextSubset, ref result);

                index++;
            }
        }
    }
}
