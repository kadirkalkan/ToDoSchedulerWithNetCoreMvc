using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PitonProject.Models.Entities
{
    public class TaskClass
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual TaskType TaskType { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public virtual User CreatedUser { get; set; }

    }
}
