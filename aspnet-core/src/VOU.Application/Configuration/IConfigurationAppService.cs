using System.Threading.Tasks;
using VOU.Configuration.Dto;

namespace VOU.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
