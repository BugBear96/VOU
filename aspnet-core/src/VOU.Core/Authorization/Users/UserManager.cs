using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using VOU.Authorization.Roles;
using Abp.Timing;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace VOU.Authorization.Users
{
    public class UserManager : AbpUserManager<Role, User>
    {

        private const string Secret = "MxkeM3bWgCef9eht85l7mbuWbX0ayq2fTaerRZCmojDlHXyZP57svrcGuxDBsylw";
        private readonly IRepository<User, long> _userRepository;

        public UserManager(
            RoleManager roleManager,
            UserStore store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<User> passwordHasher, 
            IEnumerable<IUserValidator<User>> userValidators, 
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<User>> logger, 
            IPermissionManager permissionManager, 
            IUnitOfWorkManager unitOfWorkManager, 
            ICacheManager cacheManager, 
            IRepository<OrganizationUnit, long> organizationUnitRepository, 
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, 
            IOrganizationUnitSettings organizationUnitSettings, 
            ISettingManager settingManager,
            IRepository<User, long> userRepository)
            : base(
                roleManager, 
                store, 
                optionsAccessor, 
                passwordHasher, 
                userValidators, 
                passwordValidators, 
                keyNormalizer, 
                errors, 
                services, 
                logger, 
                permissionManager, 
                unitOfWorkManager, 
                cacheManager,
                organizationUnitRepository, 
                userOrganizationUnitRepository, 
                organizationUnitSettings, 
                settingManager)
        {
            _userRepository = userRepository;
        }

        public async Task<long> CreateUserAndGetIdAsync(User user)
        {
            return await _userRepository.InsertAndGetIdAsync(user);
        }

        public string GenerateTempToken(string subject)
        {
            var h1 = Clock.Now.Hour;
            var hash = h1 + subject + Secret;
            return ConvertToNumber(ComputeMD5(hash).Substring(0, 5));
        }

        public bool VerifyTempToken(string subject, string token)
        {
            var h1 = Clock.Now.Hour;
            var hash1 = h1 + subject + Secret;
            hash1 = ConvertToNumber(ComputeMD5(hash1).Substring(0, 5));

            if (hash1 == token)
                return true;
            else
            {
                var h2 = Clock.Now.AddHours(-1).Hour;
                var hash2 = h2 + subject + Secret;
                hash2 = ConvertToNumber(ComputeMD5(hash2).Substring(0, 5));
                if (hash2 == token)
                    return true;
            }
            return false;
        }

        private string ConvertToNumber(string str)
        {
            var newStr = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (Char.IsNumber(c))
                    newStr.Append(c);
                else
                    newStr.Append(((int)c % 9));
            }
            return newStr.ToString();
        }

        private string ComputeMD5(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

    }
}
