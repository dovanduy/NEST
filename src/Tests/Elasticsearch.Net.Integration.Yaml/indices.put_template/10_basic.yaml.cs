using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace Elasticsearch.Net.Integration.Yaml.IndicesPutTemplate1
{
	public partial class IndicesPutTemplate1YamlTests
	{	


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class PutTemplate1Tests : YamlTestsBase
		{
			[Test]
			public void PutTemplate1Test()
			{	

				//do indices.put_template 
				_body = new {
					template= "test-*",
					settings= new {
						number_of_shards= "1",
						number_of_replicas= "0"
					}
				};
				this.Do(()=> _client.IndicesPutTemplateForAll("test", _body));

				//do indices.get_template 
				this.Do(()=> _client.IndicesGetTemplateForAll("test"));

				//match _response.test.template: 
				this.IsMatch(_response.test.template, @"test-*");

				//match _response.test.settings: 
				this.IsMatch(_response.test.settings, new Dictionary<string, object> {
					{ @"index.number_of_shards", @"1" },
					{ @"index.number_of_replicas", @"0" }
				});

			}
		}
	}
}

