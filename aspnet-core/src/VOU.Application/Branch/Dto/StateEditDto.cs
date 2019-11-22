using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VOU.Branch.Dto
{
    [AutoMapFrom(typeof(State))]
    public class StateEditDto : EntityDto, IShouldNormalize
    {
        [Required, StringLength(City.MaxStringLength)]
        public string StateName { get; set; }

        public List<City> Cities { get; set; }

        public void Normalize()
        {
            if (Cities == null)
                Cities = new List<City>();
        }
    }
}
