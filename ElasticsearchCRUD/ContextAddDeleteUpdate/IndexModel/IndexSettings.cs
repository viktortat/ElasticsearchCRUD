﻿namespace ElasticsearchCRUD.ContextAddDeleteUpdate.IndexModel
{
	public class IndexSettings : IndexUpdateSettings
	{
		private int _numberOfShards;
		private bool _numberOfShardsSet;

		public int NumberOfShards
		{
			get { return _numberOfShards; }
			set
			{
				_numberOfShards = value;
				_numberOfShardsSet = true;
			}
		}

		public override void WriteJson(ElasticsearchCrudJsonWriter elasticsearchCrudJsonWriter)
		{
			WriteValue("number_of_shards", _numberOfShards, elasticsearchCrudJsonWriter, _numberOfShardsSet);
			base.WriteJson(elasticsearchCrudJsonWriter);

		}
	}
}
