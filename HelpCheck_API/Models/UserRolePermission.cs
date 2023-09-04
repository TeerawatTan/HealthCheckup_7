using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class UserRolePermission
    {
        [Key]
        public int ID { get; set; }
        public string UserID { get; set; }
        public int ModuleID { get; set; }
        public bool IsActive { get; set; }
    }
}