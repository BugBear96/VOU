using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.MultiTenancy.Dto
{
    public class GetTenantsInput
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
    }
    
}
