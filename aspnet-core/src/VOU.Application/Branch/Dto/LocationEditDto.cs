using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.Branch.Dto
{
    [AutoMapFrom(typeof(Location))]
    public class LocationEditDto: Entity //, IShouldNormalize
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Postcode { get; set; }

        public CityDto City { get; set; }

        public StateDto State { get; set; }

        public string Remarks { get; set; }

        public long? CoverPictureId { get; set; }

        [IgnoreMap]
        public TimeTableSettings TimeTableJson { get; set; }

        //public List<BranchWithVoucherPlatform> VoucherPlatforms { get; set; }

        //public List<Voucher.VoucherPlatform> _voucherPlatforms { get; set; }

        /*
        public void Normalize()
        {
            if (VoucherPlatforms == null)
                VoucherPlatforms = new List<BranchWithVoucherPlatform>();
        }
        */


    }
}
