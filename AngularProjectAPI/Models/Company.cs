using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        public string Description { get; set; }
        public string BackgroundColor { get; set; }
        public string Color { get; set; }
        public string ImagePath { get; set; }
        [JsonIgnore]
        public ICollection<User> Users { get; set; }

    }
}
