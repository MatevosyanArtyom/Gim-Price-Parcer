using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects
{
    internal class SequenceCounterDo
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public long Counter { get; set; }
    }
}