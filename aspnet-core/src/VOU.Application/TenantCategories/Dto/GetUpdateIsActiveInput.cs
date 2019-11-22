using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.TenantCategories.Dto
{
    public class GetUpdateIsActiveInput
    {
        public int CategoryId { get; set; }
        public Boolean isActive { get; set; }

        public GetUpdateIsActiveInput(int _id, Boolean _isActive)
        {
            CategoryId = _id;
            isActive = _isActive;
        }
    }
}
