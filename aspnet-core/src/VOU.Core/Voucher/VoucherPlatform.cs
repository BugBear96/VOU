using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VOU.MultiTenancy;

namespace VOU.Voucher
{
    public class VoucherPlatform: AuditedAggregateRoot, IHasCreationTime, IHasModificationTime, IMustHaveTenant
    {
        public const int MaxNameLength = 40;
        public const int MaxDescriptionLength = 256;


        public VoucherPlatform()
        {
            // EMPTY
        }

        public VoucherPlatform(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));
            
            Name = name;
            //IsActive = true;
        }

        [Index("IX_VoucherScheme_TenantId_Name_ArchivedTime]", IsUnique = true, Order = 0)]
        public virtual int TenantId { get; set; }

        [ForeignKey(nameof(TenantId))]
        public virtual Tenant Tenant { get; set; }

        [Index("IX_VoucherScheme_TenantId_Name_ArchivedTime", IsUnique = true, Order = 1)]
        [Required, StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual long? CoverPictureId { get; set; }

        public virtual decimal CashValue { get; set; }

        public virtual string TermConditionJson { get; set; }

        public virtual decimal Discount { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string GiftDescription { get; set; }

        [Index("IX_VoucherScheme_TenantId_Name_ArchivedTime", IsUnique = true, Order = 2)]
        public virtual DateTime? ArchivedTime { get; set; }

        public virtual long? ArchivedByUserId { get; set; }

        //public DateTime CreationTime { get; set; }
        //public DateTime? LastModificationTime { get; set; }
        //public virtual bool IsActive { get; set; }

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

        public void UpdateName(string name)
        {
            EnsureNotArchived();

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            Name = name;
        }


        public VoucherSettings GetSettings()
        {
            if (TermConditionJson == null)
                return new VoucherSettings();

            return JsonConvert.DeserializeObject<VoucherSettings>(TermConditionJson);
        }

        public void UpdateSettings(VoucherSettings settings)
        {
            EnsureNotArchived();

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            TermConditionJson = JsonConvert.SerializeObject(settings);
        }

        private void EnsureNotArchived()
        {
            if (ArchivedTime != null)
                throw new InvalidOperationException("Voucher Platform is archived");
        }

        public void UpdateCoverPicture(long id)
        {
            CoverPictureId = id;
        }

    }
}
