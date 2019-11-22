using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Text;
using VOU.MultiTenancy;

namespace VOU.Branch
{
    [Table("Location")]
    public class Location : Entity, IHasCreationTime, IHasModificationTime, IMustHaveTenant
    {
        public const int MaxNameLength = 40;
        public const int MaxAddressLength = 255;
        public const int MaxPostCodeLength = 10;

        public Location()
        {
            // EMPTY
        }

        public Location(string name)
        {
            Name = name;
            //VoucherPlatforms = new List<BranchWithVoucherPlatform>();
        }

        public virtual int TenantId { get; set; }
        
        [ForeignKey(nameof(TenantId))]
        public virtual Tenant Tenant { get; set; }

        [Required, StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MaxAddressLength)]
        public virtual string Address { get; set; }

        [StringLength(MaxPostCodeLength)]
        public virtual string Postcode { get; set; }

        public City City { get; set; }

        public State State { get; set; }

        public virtual long? CoverPictureId { get; set; }

        public virtual string Remarks { get; set; }

        //public virtual DbGeography Coordinate { get; set; }

        //public List<BranchWithVoucherPlatform> VoucherPlatforms { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }


        public void UpdateAddress(string address, string postcode)
        {
            Address = address;
            Postcode = postcode;
        }

        public void UpdateState(State state)
        {
            State = state;
        }

        public void UpdateCity(City city)
        {
            City = city;
        }

        public void UpdateCoverPicture(long id)
        {
            CoverPictureId = id;
        }
        /*
        public void AddVoucherPlatform(
            BranchWithVoucherPlatform platform)
        {
            VoucherPlatforms.Add(platform);
        }

        public void ClearVoucherPlatforms()
        {
            VoucherPlatforms.Clear();
        }
        */
    }
}
