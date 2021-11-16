using System;
using Gim.PriceParser.Bll.Common.Entities.UserRoles;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.UserRole
{
    internal class UserRoleDo : IEntityWithIdDo, IEntityArchivableDo
    {
        public string Name { get; set; }
        public bool IsMainAdmin { get; set; }
        public DateTime CreatedDate { get; set; }
        public AccessRights AccessRights { get; set; }
        public bool IsArchived { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}