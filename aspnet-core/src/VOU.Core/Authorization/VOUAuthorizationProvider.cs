using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace VOU.Authorization
{
    public class VOUAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            var administration = context.CreatePermission(PermissionNames.Administration, L("Administration"), multiTenancySides: MultiTenancySides.Host);

            var tenant = context.CreatePermission(PermissionNames.Tenant, L("Tenant"), multiTenancySides: MultiTenancySides.Tenant);

            var states = administration.CreateChildPermission(PermissionNames.Administration_States, L("States"), multiTenancySides: MultiTenancySides.Host);
            states.CreateChildPermission(PermissionNames.Administration_States_Create, L("CreateNewState"), multiTenancySides: MultiTenancySides.Host);
            states.CreateChildPermission(PermissionNames.Administration_States_Edit, L("EditState"), multiTenancySides: MultiTenancySides.Host);
            states.CreateChildPermission(PermissionNames.Administration_States_Delete, L("DeleteState"), multiTenancySides: MultiTenancySides.Host);

            var categories = administration.CreateChildPermission(PermissionNames.Administration_Categories, L("Categories"), multiTenancySides: MultiTenancySides.Host);
            categories.CreateChildPermission(PermissionNames.Administration_Categories_Create, L("CreateNewCategory"), multiTenancySides: MultiTenancySides.Host);
            categories.CreateChildPermission(PermissionNames.Administration_Categories_Edit, L("EditCategory"), multiTenancySides: MultiTenancySides.Host);
            categories.CreateChildPermission(PermissionNames.Administration_Categories_Delete, L("DeleteCategory"), multiTenancySides: MultiTenancySides.Host);

            var branch = tenant.CreateChildPermission(PermissionNames.Branch, L("Branch"), multiTenancySides: MultiTenancySides.Tenant);
            tenant.CreateChildPermission(PermissionNames.Branch_Create, L("CreateNewBranch"), multiTenancySides: MultiTenancySides.Tenant);
            tenant.CreateChildPermission(PermissionNames.Branch_Edit, L("EditBranch"), multiTenancySides: MultiTenancySides.Tenant);
            tenant.CreateChildPermission(PermissionNames.Branch_Delete, L("DeleteBranch"), multiTenancySides: MultiTenancySides.Tenant);

            var voucherPlatform = tenant.CreateChildPermission(PermissionNames.VoucherPlatform, L("VoucherPlatform"), multiTenancySides: MultiTenancySides.Tenant);
            tenant.CreateChildPermission(PermissionNames.VoucherPlatform_Create, L("CreateNewVoucherPlatform"), multiTenancySides: MultiTenancySides.Tenant);
            tenant.CreateChildPermission(PermissionNames.VoucherPlatform_Edit, L("EditVoucherPlatform"), multiTenancySides: MultiTenancySides.Tenant);
            //tenant.CreateChildPermission(PermissionNames.VoucherPlatform_Delete, L("DeleteVoucherPlatform"), multiTenancySides: MultiTenancySides.Tenant);

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, VOUConsts.LocalizationSourceName);
        }
    }
}
