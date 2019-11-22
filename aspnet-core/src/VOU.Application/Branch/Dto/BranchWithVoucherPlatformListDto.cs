using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.Branch.Dto
{
    public class BranchWithVoucherPlatformListDto : Entity
    {
        public Voucher.Dto.VoucherPlatformListDto VoucherPlatform;

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

    }
}
