using System.ComponentModel.DataAnnotations;

namespace CommandAPI.Dtos
{
    public class CommandUpdateDto
    {
        [Required]
        [MaxLength(255)]
        public string HowTo { get; set; }

        [Required]
        public string Platform { get; set; }
        
        [Required]
        public string CommandLine { get; set; }
    }
}