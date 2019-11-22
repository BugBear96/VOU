using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AutoMapper;
using System;
using System.Collections.Generic;
using VOU.Authorization.Users;

namespace VOU.PublicUsers.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserListDto : EntityDto<long> 
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string ProfilePictureId { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }

        [IgnoreMap]
        public IList<UserRoleDto> Roles { get; set; } = new List<UserRoleDto>();
    }
}
