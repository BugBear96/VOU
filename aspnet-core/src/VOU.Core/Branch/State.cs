using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VOU.Branch
{
    [Table("State")]
    public class State : Entity, IHasCreationTime, IHasModificationTime
    {
        public const int MaxStringLength = 64;

        public State()
        {
            // EMPTY
        }
        public State(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                throw new ArgumentException("state is required", nameof(state));

            StateName = state;
            Cities = new List<City>();
        }
        [Required, StringLength(MaxStringLength)]
        public string StateName { get; set; }
        public virtual List<City> Cities { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public void UpdateState(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                throw new ArgumentException("state is required", nameof(state));

            StateName = state;
        }


        public void AddCity(
            string city)
        {
            Cities.Add(new City(city));
        }

        public void ClearCities()
        {
            Cities.Clear();
        }
    }
}
