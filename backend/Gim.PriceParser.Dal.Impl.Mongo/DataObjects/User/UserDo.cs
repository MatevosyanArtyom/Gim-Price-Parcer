using System;
using Gim.PriceParser.Bll.Common.Entities.Users;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.User
{
    internal class UserDo : IEntityWithIdDo, IEntityArchivableDo
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public bool HasSuppliersAccess { get; set; }
        public bool HasFullAccess { get; set; }
        public bool HasUsersAccess { get; set; }
        public ObjectId RoleId { get; set; }
        public ObjectId ChangePasswordToken { get; set; }
        public DateTime CreatedDate { get; set; }
        public GimUserStatus Status { get; set; }
        public bool IsArchived { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}