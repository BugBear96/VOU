using System.ComponentModel.DataAnnotations;

namespace VOU.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}