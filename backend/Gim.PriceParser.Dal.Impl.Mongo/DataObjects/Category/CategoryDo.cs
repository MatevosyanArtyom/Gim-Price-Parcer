using System.Collections.Generic;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Categories;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category
{
    internal class CategoryDo : IEntityWithIdDo, IEntityWithVersionDo
    {
        public ObjectId ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public List<CategoryMappingItem> Mappings { get; set; } = new List<CategoryMappingItem>();
        public int Position { get; set; }
        public bool IsArchived { get; set; }
        public EntityStatus Status { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
        public ObjectId Version { get; set; }
    }
}