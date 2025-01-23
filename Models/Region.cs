using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DUNWorkflow.Models
{
    public class Region
    {
        [Key]
        public string? regionCode { get; set; }
        public string? info { get; set; }
        public List<Biomes>? biomes { get; set; }  // Lista de elementos dentro de la región
        public List<Border>? borders { get; set; }  // Lista de regiones limítrofes
        
    }

    public class Biomes
    {
        public string? type { get; set; }
        public string? alignment { get; set; }
        public List<string>? placesServices { get; set; }
    }
    public class Border
    {
        public string? regionCode { get; set; }
    }
}
