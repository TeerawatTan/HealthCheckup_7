using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class Module
    {
        [Key]
        public int ID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleNameTh { get; set; }
        public string ModuleNameEn { get; set; }
        public string Route { get; set; }
        public string Icon { get; set; }
        public string IconActive { get; set; }
        public bool IsActive { get; set; }
    }
}