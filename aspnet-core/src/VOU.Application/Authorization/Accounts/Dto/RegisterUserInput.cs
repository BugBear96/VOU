
using Abp.Runtime.Validation;

namespace VOU.Authorization.Accounts.Dto
{
    public class RegisterUserInput : IShouldNormalize
    {
        public string ContactNumber { get; set; }

        public string VerificationCode { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public bool IsPhoneValid()
        {
            return !string.IsNullOrWhiteSpace(ContactNumber);
        }

        public void Normalize()
        {
            ContactNumber = ContactNumber?.Replace(" ", "");
        }
    }
}
