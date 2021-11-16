namespace Gim.PriceParser.Bll.Search
{
    public interface IElasticSearchSettings
    {
        string ConnectionString { get; set; }
        string DefaultIndex { get; set; }
    }
}