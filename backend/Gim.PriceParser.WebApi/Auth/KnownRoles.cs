using System.Collections.Generic;
using System.Security.Claims;
using Gim.PriceParser.Bll.Common.Entities.UserRoles;

// ReSharper disable EnforceIfStatementBraces

namespace Gim.PriceParser.WebApi.Auth
{
    public static class KnownRoles
    {
        public const string SuppliersReadSelf = nameof(SuppliersReadSelf);
        public const string SuppliersEditSelf = nameof(SuppliersEditSelf);
        public const string SuppliersRead = nameof(SuppliersRead);
        public const string SuppliersFull = nameof(SuppliersFull);

        public const string PriceListAdd = nameof(PriceListAdd);

        public const string PriceListsRead = nameof(PriceListsRead);
        public const string PriceListsEditSelf = nameof(PriceListsEditSelf);
        public const string PriceListsFull = nameof(PriceListsFull);
        public const string PriceListsCreateProperties = nameof(PriceListsCreateProperties);

        public const string CommitedPriceLists = nameof(CommitedPriceLists);

        public const string ProductsRead = nameof(ProductsRead);
        public const string ProductsFull = nameof(ProductsFull);

        public const string UserRolesRead = nameof(UserRolesRead);
        public const string UserRolesFull = nameof(UserRolesFull);

        public const string UsersRead = nameof(UsersRead);
        public const string UsersFull = nameof(UsersFull);

        public const string CategoriesRead = nameof(CategoriesRead);
        public const string CategoriesFull = nameof(CategoriesFull);

        public const string PropertiesRead = nameof(PropertiesRead);
        public const string PropertiesFull = nameof(PropertiesFull);

        public const string ProcessingRulesRead = nameof(ProcessingRulesRead);
        public const string ProcessingRulesFull = nameof(ProcessingRulesFull);

        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Moderator = "Moderator";
        public const string ManagerOrModerator = "Manager,Moderator";

        public static List<Claim> GetClaims(GimUserRole role)
        {
            var claims = new List<Claim>();

            var flags = role.AccessRights;

            if (flags.Suppliers.ReadSelf) claims.Add(new Claim(ClaimTypes.Role, SuppliersReadSelf));
            if (flags.Suppliers.EditSelf) claims.Add(new Claim(ClaimTypes.Role, SuppliersEditSelf));
            if (flags.Suppliers.Read) claims.Add(new Claim(ClaimTypes.Role, SuppliersRead));
            if (flags.Suppliers.Full) claims.Add(new Claim(ClaimTypes.Role, SuppliersFull));

            if (flags.PriceListAdd.Full) claims.Add(new Claim(ClaimTypes.Role, PriceListAdd));

            if (flags.PriceLists.Read) claims.Add(new Claim(ClaimTypes.Role, PriceListsRead));
            if (flags.PriceLists.EditSelf) claims.Add(new Claim(ClaimTypes.Role, PriceListsEditSelf));
            if (flags.PriceLists.Full) claims.Add(new Claim(ClaimTypes.Role, PriceListsFull));
            if (flags.PriceLists.CreateProperties) claims.Add(new Claim(ClaimTypes.Role, PriceListsCreateProperties));

            if (flags.CommitedPriceLists.Read) claims.Add(new Claim(ClaimTypes.Role, CommitedPriceLists));

            if (flags.Products.Read) claims.Add(new Claim(ClaimTypes.Role, ProductsRead));
            if (flags.Products.Full) claims.Add(new Claim(ClaimTypes.Role, ProductsFull));

            if (flags.UserRoles.Read) claims.Add(new Claim(ClaimTypes.Role, UserRolesRead));
            if (flags.UserRoles.Full) claims.Add(new Claim(ClaimTypes.Role, UserRolesFull));

            if (flags.Users.Read) claims.Add(new Claim(ClaimTypes.Role, UsersRead));
            if (flags.Users.Full) claims.Add(new Claim(ClaimTypes.Role, UsersFull));

            if (flags.Categories.Read) claims.Add(new Claim(ClaimTypes.Role, CategoriesRead));
            if (flags.Categories.Full) claims.Add(new Claim(ClaimTypes.Role, CategoriesFull));

            if (flags.Properties.Read) claims.Add(new Claim(ClaimTypes.Role, PropertiesRead));
            if (flags.Properties.Full) claims.Add(new Claim(ClaimTypes.Role, PropertiesFull));

            if (flags.ProcessingRules.Read) claims.Add(new Claim(ClaimTypes.Role, ProcessingRulesRead));
            if (flags.ProcessingRules.Full) claims.Add(new Claim(ClaimTypes.Role, ProcessingRulesFull));

            return claims;
        }
    }
}