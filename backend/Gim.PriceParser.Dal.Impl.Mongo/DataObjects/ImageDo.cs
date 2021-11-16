using Gim.PriceParser.Bll.Common.Entities.Images;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects
{
    internal class ImageDo : IEntityWithIdDo
    {
        public ObjectId ProductId { get; set; }
        public bool IsMain { get; set; }
        public bool IsPublished { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Size { get; set; }
        public byte[] Data { get; set; }
        public GimImageDownloadStatus Status { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}