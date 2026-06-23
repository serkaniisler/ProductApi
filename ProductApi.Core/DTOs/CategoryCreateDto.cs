using System.ComponentModel.DataAnnotations;

namespace ProductApi.Core.DTOs
{
    public class CategoryCreateDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}