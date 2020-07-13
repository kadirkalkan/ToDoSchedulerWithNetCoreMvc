using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PitonProject.Models.Enums
{
    public class EnumClass
    {

        public enum TaskType
        {
            day,
            week,
            month
        }

        public enum LogType
        {
            info,
            error
        }

        public enum ProcessType
        {
            initialize,
            @new,
            info,
            danger,
            success,
            warning,
            rose,
            primary
        }
    }
}
