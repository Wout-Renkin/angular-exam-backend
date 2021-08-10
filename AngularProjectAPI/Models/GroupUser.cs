using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class GroupUser
    {
        public int GroupUserId { get; set; }
        public Boolean Moderator { get; set; }
        public Boolean RequestedModerator { get; set; }
        public Boolean GroupRequest { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int CompanyId { get; set; }
        public Company company { get; set; }

    }
}
