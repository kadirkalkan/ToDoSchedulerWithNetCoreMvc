using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PitonProject.Models.Entities
{
    [Table("LOG")]
    public class Log
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [DisplayName("Log")]
        public string LogText { get; set; }

        public string ClassName { get; set; }
        public string MethodName { get; set; }

        [Required]
        public virtual LogType LogType { get; set; }

        private DateTime? createdTime = DateTime.Now;
        public DateTime CreatedTime
        {
            get { return this.createdTime.HasValue ? this.createdTime.Value : DateTime.Now; }
            set { this.createdTime = value; }
        }

    }
}
