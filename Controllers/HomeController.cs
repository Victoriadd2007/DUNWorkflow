using System.Diagnostics;
using System.Globalization;
using DUNWorkflow.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DUNWorkflow.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var currentCulture = CultureInfo.CurrentUICulture.Name; // "es", "en", etc.
            //string jsonFileName = currentCulture.StartsWith("es") ? "Guia_es.json" : currentCulture.StartsWith("de") ? "Guia_de.json" : "Guia_en.json";
            string jsonFileName = currentCulture.StartsWith("es") ? "Guia_es.json" : "Guia_en.json";


            // Cargar el archivo JSON y mostrar la primera tarjeta
            string jsonFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "json", jsonFileName);
            string jsonData = System.IO.File.ReadAllText(jsonFilePath);
            var cardDataArray = JsonConvert.DeserializeObject<CardData[]>(jsonData);
            ViewBag.CardList = cardDataArray; // Lista completa
            var firstCard = cardDataArray.FirstOrDefault();

            //visitas
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "visitcount.txt");

            // Leer el número de visitas
            int count = 0;
            if (System.IO.File.Exists(filePath))
            {
                count = int.Parse(System.IO.File.ReadAllText(filePath));
            }

            // Incrementar y guardar
            count++;
            System.IO.File.WriteAllText(filePath, count.ToString());


            return View(firstCard);
        }

        [HttpGet]
        public IActionResult LoadCard(string codeNumber)
        {
            // Ruta al archivo JSON
            var currentCulture = CultureInfo.CurrentUICulture.Name; // "es", "en", etc.
            string jsonFileName = currentCulture.StartsWith("es") ? "Guia_es.json" : "Guia_en.json";
            string jsonFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "json", jsonFileName);


            // Leer el JSON
            string jsonData = System.IO.File.ReadAllText(jsonFilePath);

            // Deserializar como array
            var cardDataArray = JsonConvert.DeserializeObject<CardData[]>(jsonData);

            // Buscar el objeto correspondiente al `codeNumber`
            var cardData = cardDataArray.FirstOrDefault(card => card.CodeNumber == codeNumber);

            if (cardData == null)
            {
                return NotFound(); // Manejo de error si no se encuentra el código
            }


            // Pasar el modelo a la vista parcial
            return PartialView("_CardPartial", cardData);
        }
    }
}
