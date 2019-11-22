using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.Branch.Dto
{
    public class StateListDto : EntityDto
    {
        public string StateName { get; set; }

        public List<CityDto> Cities { get; set; }
    }
}
