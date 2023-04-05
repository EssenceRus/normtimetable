
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_MED.Models
{
    public class WorkTime
    {
        [Key]
        public int WorkTimeId { get; set; }

        public virtual DayOfWeek DayNavigation { get; set; } = null!;

        public DateTime WorkStart { get; set; }

        public DateTime WorkEnd { get; set; }

        public bool IsWorking { get; set; }

        public DateTime LunchStart { get; set;}

        public DateTime LunchEnd { get; set;}

        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}
