using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.TenantCategories.Dto
{
    public class GetTenantCategoryInput
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }

    }
}
