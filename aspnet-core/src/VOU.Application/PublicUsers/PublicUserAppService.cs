using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Extensions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VOU.Authorization.Users;
using VOU.Dto;
using VOU.PublicUsers.Dto;
using Abp.ObjectMapping;

namespace VOU.PublicUsers
{
    //[AbpAuthorize(PermissionNames.PublicUsers)]
    public class PublicUserAppService : VOUAppServiceBase, IPublicUserAppService
    {
        private readonly UserManager _userManager;
        private readonly IObjectMapper _objectMapper;


        public PublicUserAppService(
            
            UserManager userManager,
            IObjectMapper objectMapper)
        {

            _userManager = userManager;
            _objectMapper = objectMapper;
        }

        public async Task<PagedResultDto<UserListDto>> GetPublicUsers(PagedResultInput input)
        {

            var cursor = _userManager.Users.Where(x => x.UserType == UserType.Public).AsQueryable();
            var totalCount = await cursor.CountAsync();

            if (!input.Filter.IsNullOrEmpty())
            {
                cursor = cursor
                    .Where(x =>
                        x.UserName.Contains(input.Filter) ||
                        x.Name.Contains(input.Filter) ||
                        x.EmailAddress.Contains(input.Filter));
            }

            var orderedCursor = !input.Sorting.IsNullOrEmpty()
                ? cursor.OrderBy(x => input.Sorting) : cursor.OrderBy(x => x.Id);

            cursor = input.MaxResultCount >= 0
                ? orderedCursor.Skip(input.SkipCount).Take(input.MaxResultCount)
                : orderedCursor;

            var users = await cursor.Include(x => x.Roles).ToListAsync();

            var records = new List<UserListDto>();
            foreach (var user in users)
            {
                var dto = _objectMapper.Map(user, new UserListDto());

                if (!user.Roles.Any())
                {
                    records.Add(dto);
                }
            }

            return new PagedResultDto<UserListDto>(totalCount, records);


        }
    }
}
