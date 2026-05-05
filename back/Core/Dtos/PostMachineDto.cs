using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class PostMachineDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ConnectionString { get; set; }
        [Required]
        public string Component { get; set; }
    }
}
