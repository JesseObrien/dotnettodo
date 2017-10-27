using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace todoapi.Models
{
    public class Tag
    {

        public Tag()
        {

        }

        [Key]
        public int TagID { get; set; }

        public string TagName { get; set; }

        public long? TodoID { get; set; }

        [ForeignKey("TodoID")]
        public virtual TodoItem ToDo { get; set; }
    }
}
