using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Group
    {

        public int GroupId { get; set; }
        public string Name { get; set; }
        public string ThemeColor { get; set; }
        public string ImagePath { get; set; }
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<Post> Posts { get; set; }

    }
}
