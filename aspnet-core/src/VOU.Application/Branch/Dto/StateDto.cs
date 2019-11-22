using Abp.AutoMapper;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.Branch.Dto
{
    [AutoMapFrom(typeof(State))]
    public class StateDto : Entity
    {
        
         public string StateName { get; set; }
        
    }
}
