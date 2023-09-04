using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class Role
    {
        [Key]
        public int ID { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }
}