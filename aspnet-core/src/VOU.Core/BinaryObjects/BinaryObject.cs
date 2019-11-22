using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace VOU.BinaryObjects
{
    public class BinaryObject : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public const int MaxTypeLength = 40;
        public const int MaxContentTypeLength = 127;

        public virtual int? TenantId { get; set; }

        [Required]
        [StringLength(MaxTypeLength)]
        public virtual string Type { get; set; }

        [Required]
        public virtual byte[] Content { get; set; }

        [Required]
        [StringLength(MaxContentTypeLength)]
        public virtual string ContentType { get; set; }
    }
}