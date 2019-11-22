using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VOU.Branch
{
    [Table("City")]
    public class City : Entity, IHasCreationTime, IHasModificationTime
    {
        public const int MaxStringLength = 64;

        public City()
        {
            // EMPTY
        }
        public City(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("city is required", nameof(city));

            CityName = city;
        }
        [Required, StringLength(MaxStringLength)]
        public string CityName { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
