using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class ArticleStatus
    {
        public int ArticleStatusID { get; set; }
        public string Name { get; set; }

        //Relations
        public ICollection<Article> Articles { get; set; }
    }
}
