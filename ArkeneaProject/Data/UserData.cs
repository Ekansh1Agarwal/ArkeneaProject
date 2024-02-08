using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArkeneaProject.Data
{
    [Table("UserData")]
    public class UserData
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required] 
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public string fileName { get; set; }
        public string fileType { get; set; }
        [Required]
        public byte[] file { get; set; }

    }
}
