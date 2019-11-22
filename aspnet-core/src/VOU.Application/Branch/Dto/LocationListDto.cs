using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VOU.MultiTenancy;

namespace VOU.Branch.Dto
{
    public class LocationListDto : EntityDto
    {
        //public TenantDto Tenant { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Postcode { get; set; }

        public CityDto City { get; set; }

        public StateDto State { get; set; }

        public long? CoverPictureId { get; set; }

        public string Remarks { get; set; }
    }
}
