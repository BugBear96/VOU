using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Configuration;
using Abp.UI;
using Abp.Zero.Configuration;
using VOU.Authorization.Accounts.Dto;
using VOU.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.Identity;
using VOU.Users.Dto;

namespace VOU.Authorization.Accounts
{
    public class AccountAppService : VOUAppServiceBase, IAccountAppService
    {
        // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

        private readonly UserRegistrationManager _userRegistrationManager;

        public AccountAppService(
            UserRegistrationManager userRegistrationManager)
        {
            _userRegistrationManager = userRegistrationManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
             var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
            );

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };        
        }

        public async Task<UserDto> GetOrUpdateUserInfo(UserUpdateDto input)
        {
               var user = await UserManager.FindByIdAsync((AbpSession.UserId ?? 0).ToString());
            if (user == null)
                throw new UserFriendlyException(L("InvalidUser"));

            if (!string.IsNullOrEmpty(input.FcmToken))
            {
                user.FcmToken = input.FcmToken;
                await UserManager.UpdateAsync(user);
            }

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                EmailAddress = user.EmailAddress,
            };
        }

        [AbpAllowAnonymous]
        public async Task RegisterUser(RegisterUserInput input)
        {
            if (!UserManager.VerifyTempToken(input.ContactNumber, input.VerificationCode))
                throw new UserFriendlyException(L("InvalidPhoneVerificationCode"));

            if (string.IsNullOrWhiteSpace(input.Name) || string.IsNullOrWhiteSpace(input.Email))
                throw new UserFriendlyException(L("MissingFields"));

            if (await UserManager.Users.AnyAsync(x => x.EmailAddress == input.Email))
                throw new UserFriendlyException(L("EmailExists"));

            var user = User.CreateAppUser(input.ContactNumber, input.Name, input.Email, input.Password);
            user.SetNormalizedNames();
            var userId = await UserManager.CreateUserAndGetIdAsync(user);

            await CurrentUnitOfWork.SaveChangesAsync();
            
        }


        [AbpAllowAnonymous]
        public async Task<string> RegisterPhone(RegisterUserInput input)
        {
#if true
            if (!input.IsPhoneValid())
                throw new UserFriendlyException(L("InvalidPhoneNumber"));

            if (await UserManager.Users.AnyAsync(x => x.PhoneNumber == input.ContactNumber))
                throw new UserFriendlyException(L("PhoneNumberExists"));

            return await Task.Run(() => UserManager.GenerateTempToken(input.ContactNumber));

#else
            return await Task.Run(() => string.Empty);
#endif
        }

        [AbpAllowAnonymous]
        public async Task<bool> VerifyPhone(RegisterUserInput input)
        {
            return await Task.Run(() => UserManager.VerifyTempToken(input.ContactNumber, input.VerificationCode));
        }

    }
}
