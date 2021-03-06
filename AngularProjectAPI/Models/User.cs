using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class User
    {


        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string Token { get; set; }

        //Relations
        public int RoleId { get; set; }
        public Role Role { get; set; }


        public int? CompanyId { get; set; }
        public Company? Company { get; set; }



    }
}
