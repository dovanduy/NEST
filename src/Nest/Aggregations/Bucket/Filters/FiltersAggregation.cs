using System;
using System.Collections.Generic;
using System.Linq;
using Nest.Resolvers.Converters.Aggregations;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(FiltersAggregatorConverter))]
	public interface IFiltersAggregator : IBucketAggregator
	{
		[JsonProperty("filters")]
		IEnumerable<IQueryContainer> Filters { get; set; }
	}

	public class FiltersAggregator : BucketAggregator, IFiltersAggregator
	{
		public IEnumerable<IQueryContainer> Filters { get; set; }
	}

	public class FiltersAggregatorDescriptor<T> 
		: BucketAggregatorBaseDescriptor<FiltersAggregatorDescriptor<T>, IFiltersAggregator, T>
		, IFiltersAggregator
		where T : class
	{
		IEnumerable<IQueryContainer> IFiltersAggregator.Filters { get; set; }

		public FiltersAggregatorDescriptor<T> Filters(params Func<QueryDescriptor<T>, QueryContainer>[] queryDescriptors) =>
			Assign(a=>a.Filters = queryDescriptors?.Select(f => f?.Invoke(new QueryDescriptor<T>()))?.ToListOrNullIfEmpty());

	}
}