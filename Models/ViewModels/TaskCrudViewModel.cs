using PitonProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PitonProject.Models.ViewModels
{
    public class TaskCrudViewModel
    {
        public TaskClass task { get; set; }
        public string formattedStartDate { get; set; }
    }
}
