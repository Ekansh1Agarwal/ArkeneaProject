using ArkeneaProject.Data;

namespace ArkeneaProject.Models
{
    public class FileUpload
    {
        public IFormFile file { get; set; }
        public string User { get; set; }
    }
}
