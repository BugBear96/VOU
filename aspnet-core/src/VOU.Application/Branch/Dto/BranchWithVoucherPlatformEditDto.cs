using Abp.AutoMapper;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using VOU.Voucher;
using VOU.Voucher.Dto;

namespace VOU.Branch.Dto
{
    [AutoMapFrom(typeof(BranchWithVoucherPlatform))]
    public class BranchWithVoucherPlatformEditDto : Entity
    {
        public int LocationId { get; set; }

        public int VoucherPlatformId { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        //public DateTime? ArchivedTime { get; set; }

        //public long? ArchivedByUserId { get; set; }

    }
}
