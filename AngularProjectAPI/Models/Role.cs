using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleID { get; set; }
        public string Name { get; set; }

        //Relations
        public ICollection<User> Users { get; set; }
    }
}
