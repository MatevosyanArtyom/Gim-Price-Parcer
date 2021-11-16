namespace Gim.PriceParser.Bll.Search
{
    public class ElasticSearchSettings: IElasticSearchSettings
    {
        public string ConnectionString { get; set; }
        public string DefaultIndex { get; set; }
    }
}