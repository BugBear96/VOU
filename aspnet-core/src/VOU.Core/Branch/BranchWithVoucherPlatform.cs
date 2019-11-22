using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VOU.MultiTenancy;
using VOU.Voucher;

namespace VOU.Branch
{
    [Table("BranchWithVoucherPlatform")]
    public class BranchWithVoucherPlatform : AuditedAggregateRoot, IMustHaveTenant
    {
        public BranchWithVoucherPlatform()
        {
            // EMPTY
        }

        public BranchWithVoucherPlatform(Location location, VoucherPlatform voucherPlatform)
        {
            Location = location;
            VoucherPlatform = voucherPlatform;
        }

        public virtual int TenantId { get; set; }

        [ForeignKey(nameof(TenantId))]
        public virtual Tenant Tenant { get; set; }

        public Location Location { get; set; }

        public VoucherPlatform  VoucherPlatform { get; set; }

        public virtual DateTime Start { get; set; }

        public virtual DateTime End { get; set; }

        public virtual DateTime? ArchivedTime { get; set; }

        public virtual long? ArchivedByUserId { get; set; }

        public void Archive(long? archivedByUserId)
        {
            if (ArchivedTime == null)
            {
                ArchivedTime = Clock.Now;
                ArchivedByUserId = archivedByUserId;
            }
        }

        public void Activate()
        {
            if (ArchivedTime == null)
                return;

            ArchivedTime = null;
            ArchivedByUserId = null;
        }

        public void UpdateTime(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
}
