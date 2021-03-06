using System.Collections.Generic;
using ElasticsearchCRUD.Model;
using ElasticsearchCRUD.Model.SearchModel;
using ElasticsearchCRUD.Model.SearchModel.Queries;
using ElasticsearchCRUD.Tracing;
using System;
using Xunit;

namespace ElasticsearchCRUD.Integration.Test.SearchTests
{
	public class SearchQueryQueryStringQueryTest : SetupSearch, IDisposable
    {
        public SearchQueryQueryStringQueryTest()
        {
            Setup();
        }

        public void Dispose()
        {
            TearDown();
        }

        [Fact]
		public void SearchQueryQueryStringQuery()
		{
			var search = new Search { Query = new Query(new QueryStringQuery("one document")) };

			using (var context = new ElasticsearchContext(ConnectionString, ElasticsearchMappingResolver))
			{
				Assert.True(context.IndexTypeExists<SearchTest>());
				var items = context.Search<SearchTest>(search);
				Assert.Equal(2, items.PayloadResult.Hits.Total);
			}
		}

		[Fact]
		public void SearchQueryQueryStringQueryAllProperties()
		{
			var search = new Search
			{
				Query = new Query(new QueryStringQuery("one document")
				{
					AllowLeadingWildcard = true,
					Analyzer = LanguageAnalyzers.German,
					AnalyzeWildcard = false,
					AutoGeneratePhraseQueries = false,
					Boost = 2.1,
					DefaultField = "_all",
					DefaultOperator = QueryDefaultOperator.OR,
					EnablePositionIncrements = true,
					Fuzziness = 0.5,
					FuzzyMaxExpansions = 49,
					FuzzyPrefixLength = 0,
					Lenient = false,
					Locale = "ROOT"
				})
			};

			using (var context = new ElasticsearchContext(ConnectionString, ElasticsearchMappingResolver))
			{
				Assert.True(context.IndexTypeExists<SearchTest>());
				var items = context.Search<SearchTest>(search);
				Assert.Equal(2, items.PayloadResult.Hits.Total);
			}
		}

		[Fact]
		public void SearchQueryQueryStringQueryMultipleFields()
		{
			var search = new Search
			{
				Query = new Query(new QueryStringQuery("one document")
				{
					Fields = new List<string> { "name", "details" },
					TieBreaker = 1,
					UseDisMax = true
				})
			};

			using (var context = new ElasticsearchContext(ConnectionString, ElasticsearchMappingResolver))
			{
				Assert.True(context.IndexTypeExists<SearchTest>());
				var items = context.Search<SearchTest>(search);
				Assert.Equal(2, items.PayloadResult.Hits.Total);
			}
		}


		[Fact]
		public void SearchQuerySimpleQueryStringQuery()
		{
			var search = new Search
			{
				Query = new Query(new SimpleQueryStringQuery("doc*")
				{
					Analyzer = DefaultAnalyzers.Snowball,
					DefaultOperator = QueryDefaultOperator.OR,
					Flags = SimpleQueryFlags.ALL,
					Lenient = true,
					LowercaseExpandedTerms = true,
					Fields = new List<string> { "details" }
				})
			};

			using (var context = new ElasticsearchContext(ConnectionString, ElasticsearchMappingResolver))
			{
				context.TraceProvider = new ConsoleTraceProvider();
				Assert.True(context.IndexTypeExists<SearchTest>());
				var items = context.Search<SearchTest>(search);
				Assert.Equal(2, items.PayloadResult.Hits.Total);
			}
		}
	}
}