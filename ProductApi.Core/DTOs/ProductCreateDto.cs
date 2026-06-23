using System.ComponentModel.DataAnnotations;

namespace ProductApi.Core.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalı")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stok 0 veya daha büyük olmalı")]
        public int Stock { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}