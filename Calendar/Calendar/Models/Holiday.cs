using System.ComponentModel.DataAnnotations;

namespace Calendar.Models
{
    public class Holiday
    {
        [Key]
        public int? Number { get; set; }
        [DataType(DataType.Date)]
        public DateTime? holidays {  get; set; }
    }
}
