using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Body { get; set; }

        //Relations
        public int? UserId { get; set; }
        public User User { get; set; } 
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }


    }
}
