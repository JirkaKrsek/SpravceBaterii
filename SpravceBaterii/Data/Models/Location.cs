using System.ComponentModel.DataAnnotations;

namespace SpravceBaterii.Data.Models
{
    /// <summary>
    /// Modelová třída pro umístění
    /// </summary>
    public class Location
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vyplňte název")]
        [StringLength(100, ErrorMessage = "Název může mít max. 100 znaků")]
        public string Name { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Popis může mít max. 200 znaků")]
        public string? Description { get; set; }

        [StringLength(100, ErrorMessage = "Podlaží může mít max. 100 znaků")]
        public string? Floor { get; set; }

        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public ICollection<Device>? Devices { get; set; }
    }
}
