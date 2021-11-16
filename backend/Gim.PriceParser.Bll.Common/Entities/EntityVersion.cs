using System;
using Gim.PriceParser.Bll.Common.Entities.Users;

namespace Gim.PriceParser.Bll.Common.Entities
{
    public class EntityVersion<T>
    {
        public T Entity { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public GimUser User { get; set; }
        public string Id { get; set; }
        public long SeqId { get; set; }
    }
}