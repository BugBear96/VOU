using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VOU.MultiTenancy;

namespace VOU.TenantCategories
{
    [Table("TenantCategory")]
    public class TenantCategory : Entity, IHasCreationTime, IHasModificationTime
    {
        public const int MaxTitleLength = 256;

        public TenantCategory()
        {
            // EMPTY
        }
        public TenantCategory(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Name is required", nameof(title));

            Title = title;
            isActive = true;
            SubCategories = new List<TenantSubCategory>();
        }

        [Required]
        [StringLength(MaxTitleLength)]
        public string Title { get; set; }

        public Boolean isActive { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public virtual List<TenantSubCategory> SubCategories { get; set; }

        public void UpdateName(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required", nameof(title));

            Title = title;
        }

        public void UpdateIsActive(Boolean flag)
        {
            isActive = flag;
        }

        public void AddSubCategories(
            string title)
        {
            SubCategories.Add(new TenantSubCategory(title));
        }

        public void ClearSubCategories()
        {
            SubCategories.Clear();
        }
    }
}
