namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions
{
    internal interface IEntityArchivableDo
    {
        bool IsArchived { get; set; }
    }
}