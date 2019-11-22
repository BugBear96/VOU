using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VOU.Branch.Dto
{
    [AutoMapFrom(typeof(City))]
    public class CityDto
    {
        [Required, StringLength(City.MaxStringLength)]
        public string CityName { get; set; }

    }
}
