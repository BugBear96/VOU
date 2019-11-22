using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VOU.Voucher.Dto
{
    [AutoMapFrom(typeof(VoucherPlatform))]
    public class VoucherPlatformEditDto : EntityDto, IShouldNormalize
    {
        [Required, StringLength(VoucherPlatform.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(VoucherPlatform.MaxDescriptionLength)]
        public string Description { get; set; }

        public decimal CashValue { get; set; }

        [IgnoreMap]
        public VoucherSettings TermConditionJson { get; set; }

        public decimal Discount { get; set; }

        [StringLength(VoucherPlatform.MaxDescriptionLength)]
        public string GiftDescription { get; set; }

        public DateTime? ArchivedTime { get; set; }

        public void Normalize()
        {
            if (TermConditionJson == null)
                TermConditionJson = new VoucherSettings();
        }
    }
}
