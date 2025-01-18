using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DUNWorkflow.Models
{
    public class Region
    {
        [Key]
        public string? regionCode { get; set; }
        public string? info { get; set; }
        public List<Element>? Elements { get; set; }  // Lista de elementos dentro de la región
        public List<string>? placesservices { get; set; }
    }

    public class Element
    {
        public string? type { get; set; } 
    }
}
