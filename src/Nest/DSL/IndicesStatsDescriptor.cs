﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Elasticsearch.Net;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Nest.Resolvers;
using Nest.Domain;

namespace Nest
{
	[DescriptorFor("IndicesStats")]
	public partial class IndicesStatsDescriptor : 
		IndicesOptionalPathDescriptor<IndicesStatsDescriptor, IndicesStatsRequestParameters>
		, IPathInfo<IndicesStatsRequestParameters>
	{
		private IEnumerable<TypeNameMarker> _Types { get; set; }
		private IEnumerable<IndicesStatsMetric> _Metrics { get; set; }
		
		//<summary>A comma-separated list of fields for `completion` metric (supports wildcards)</summary>
		public IndicesStatsDescriptor Types(params Type[] completion_fields)
		{
			this._Types = completion_fields.Select(t=>(TypeNameMarker)t);
			return this;
		}

		public IndicesStatsDescriptor Metrics(params IndicesStatsMetric[] metrics)
		{
			this._Metrics = metrics;
			return this;
		}

		ElasticsearchPathInfo<IndicesStatsRequestParameters> IPathInfo<IndicesStatsRequestParameters>.ToPathInfo(IConnectionSettingsValues settings)
		{
			var pathInfo = base.ToPathInfo<IndicesStatsRequestParameters>(settings, this._QueryString);
			if (this._Types.HasAny())
			{
				var inferrer = new ElasticInferrer(settings);
				var types = inferrer.TypeNames(this._Types);
				this._QueryString.AddQueryString("types", string.Join(",", types));
			}
			if (this._Metrics != null)
				pathInfo.Metric = this._Metrics.Cast<Enum>().GetStringValue();
			pathInfo.HttpMethod = PathInfoHttpMethod.GET;

			return pathInfo;
		}
	}
}
