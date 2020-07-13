using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PitonProject.Models.Entities
{
    [Table("PAGE")]
    public class WebPage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [DisplayName("Sayfa Adı"), StringLength(50), Required]
        public string PageName { get; set; }
    }
}
