using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class MasterTreatment
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
