using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using VOU.Authorization;

namespace VOU
{
    [DependsOn(
        typeof(VOUCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class VOUApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<VOUAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(VOUApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
