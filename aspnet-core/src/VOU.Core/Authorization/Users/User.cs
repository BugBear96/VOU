using System;
using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Extensions;
using Microsoft.AspNet.Identity;


namespace VOU.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        public virtual long? ProfilePictureId { get; set; }

        public virtual UserType UserType { get; set; }

        public virtual string FcmToken { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>(),
                UserType = UserType.Officer
            };

            user.SetNormalizedNames();

            return user;
        }

        public static User CreateAppUser(
            string contact,
            string name,
            string emailAddress,
            string password)
        {
            return new User
            {
                UserName = name,
                Name = name,
                Surname = "-",
                EmailAddress = emailAddress,
                Password = new PasswordHasher().HashPassword(password),
                IsActive = true,
                PhoneNumber = contact,
                FcmToken = null,
                UserType = UserType.Public
            };
        }
    }
}
