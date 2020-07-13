using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PitonProject.Models.Entities
{
    [Table("TASK_TYPE")]
    public class TaskType
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [DisplayName("Task Tipi")]
        public string TaskTypeText { get; set; }

    }
}
