using System.ComponentModel.DataAnnotations;

namespace SpravceBaterii.Data.Models
{
    /// <summary>
    /// Modelová třída pro chemické složení baterie
    /// </summary>
    public class ChemicalComposition
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        public ICollection<Battery>? Batteries { get; set; }
    }
}
