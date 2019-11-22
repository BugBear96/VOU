using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.Voucher.Dto
{
    public class VoucherPlatformListDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public long? CoverPictureId { get; set; }

        public decimal CashValue { get; set; }

        public string TermConditionJson { get; set; }

        public decimal Discount { get; set; }

        public string GiftDescription { get; set; }

        public DateTime? ArchivedTime { get; set; }
    }
}
