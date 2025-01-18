using DUNWorkflow.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace DUNWorkflow.Controllers
{
    public class MapController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MapController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Map()
        {
            // Ruta al archivo JSON (ajusta según la ubicación del archivo)
            string jsonFileName = "map.json";
            string jsonFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "json", jsonFileName);
            string jsonData = System.IO.File.ReadAllText(jsonFilePath);

            // Deserializar el JSON a una lista de objetos Region
            List<Region> regions = JsonConvert.DeserializeObject<List<Region>>(jsonData);

            ViewBag.RegionsJson = jsonData;

            // Pasar la lista a la vista
            return View(regions);
        }
    }
}
