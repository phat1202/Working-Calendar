using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Calendar.Models
{
    public class WorkingDay
    {
        [Key]
        public int? Number { get; set; }
        public DateTime? Date { get; set; }
        [DefaultValue(false)]
        public bool Selection { get; set; }
        [DefaultValue(false)]
        public bool IsHoliday { get; set; }
        [DefaultValue(false)]
        public bool Weekend { get; set; }
    }
}
