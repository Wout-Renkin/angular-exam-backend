using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TagID { get; set; }
        public string Name { get; set; }

        //Relations
        public ICollection<Article> Articles { get; set; }
    }
}
