using Abp.Authorization;
using VOU.Authorization.Roles;
using VOU.Authorization.Users;

namespace VOU.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
