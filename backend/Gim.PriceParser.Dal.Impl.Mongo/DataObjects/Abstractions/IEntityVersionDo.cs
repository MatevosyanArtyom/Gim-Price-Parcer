namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions
{
    internal interface IEntityVersionDo<TDo> : IEntityWithIdDo where TDo : IEntityWithIdDo, IEntityWithVersionDo
    {
        TDo Entity { get; set; }
    }
}