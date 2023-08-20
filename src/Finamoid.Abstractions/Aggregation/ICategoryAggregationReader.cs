using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finamoid.Abstractions.Aggregation
{
    public interface ICategoryAggregationReader
    {
        Task<IEnumerable<CategoryAggregation>> ReadAsync(string path);
    }
}
