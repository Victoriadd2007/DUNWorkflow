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
            // Leer el idioma desde las cookies
            var lang = Request.Cookies["LanguagePreference"] ?? "en"; // Por defecto, inglés

            // Seleccionar el archivo JSON según el idioma
            string jsonFileName = lang == "es" ? "Guia_es.json" : "Guia_en.json";

            // Cargar el archivo JSON y mostrar la primera tarjeta
            string jsonFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "json", jsonFileName);
            string jsonData = System.IO.File.ReadAllText(jsonFilePath);
            var cardDataArray = JsonConvert.DeserializeObject<GuideData[]>(jsonData);
            ViewBag.CardList = cardDataArray; // Lista completa
            var firstCard = cardDataArray.FirstOrDefault();

            // Contador de visitas
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "visitcount.txt");
            int count = 0;
            if (System.IO.File.Exists(filePath))
            {
                count = int.Parse(System.IO.File.ReadAllText(filePath));
            }
            count++;
            System.IO.File.WriteAllText(filePath, count.ToString());

            return View(firstCard);
        }


        [HttpPost]
        public IActionResult ChangeLanguage(string lang)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                // Configurar la cookie de idioma
                Response.Cookies.Append(
                    "LanguagePreference",
                    lang,
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult LoadCard(string codeNumber)
        {
            // Ruta al archivo JSON
            var lang = Request.Cookies["LanguagePreference"] ?? "en"; // Por defecto, inglés

            // Seleccionar el archivo JSON según el idioma
            string jsonFileName = lang == "es" ? "Guia_es.json" : "Guia_en.json";
            string jsonFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "json", jsonFileName);


            // Leer el JSON
            string jsonData = System.IO.File.ReadAllText(jsonFilePath);

            // Deserializar como array
            var cardDataArray = JsonConvert.DeserializeObject<GuideData[]>(jsonData);

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
