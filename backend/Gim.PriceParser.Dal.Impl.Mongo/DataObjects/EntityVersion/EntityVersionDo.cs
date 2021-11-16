using System;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.EntityVersion
{
    internal class EntityVersionDo<TVersionDo> : IEntityWithIdDo where TVersionDo : IEntityWithIdDo
    {
        public TVersionDo Entity { get; set; }
        public DateTime CreatedDate { get; set; }
        public ObjectId UserId { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}