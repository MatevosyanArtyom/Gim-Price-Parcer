using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.User;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.EntityVersion
{
    internal class EntityVersionFullDo<TDo> : EntityVersionDo<TDo> where TDo : IEntityWithIdDo
    {
        public IEnumerable<UserDo> Users { get; set; }
    }
}