using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VOU.TenantCategories
{
    [Table("TenantSubCategory")]
    public class TenantSubCategory : Entity, IHasCreationTime, IHasModificationTime
    {
        public const int MaxTitleLength = 256;

        public TenantSubCategory()
        {
            // EMPTY
        }

        public TenantSubCategory(string title)
        {
            Title = title;
        }

        [Required]
        [StringLength(MaxTitleLength)]
        public string Title { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

    }
}
