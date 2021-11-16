namespace Gim.PriceParser.Bll.Common.Entities.UserRoles
{
    public class AccessRights
    {
        public AccessRightMode Suppliers { get; set; } = new AccessRightMode();

        public AccessRightMode PriceListAdd { get; set; } = new AccessRightMode();
        public AccessRightMode PriceLists { get; set; } = new AccessRightMode();
        public AccessRightMode CommitedPriceLists { get; set; } = new AccessRightMode();

        public AccessRightMode Products { get; set; } = new AccessRightMode();

        public AccessRightMode UserRoles { get; set; } = new AccessRightMode();
        public AccessRightMode Users { get; set; } = new AccessRightMode();
        public AccessRightMode Categories { get; set; } = new AccessRightMode();
        public AccessRightMode Properties { get; set; } = new AccessRightMode();
        public AccessRightMode ProcessingRules { get; set; } = new AccessRightMode();
    }
}