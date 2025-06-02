using DUNWorkflow.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DUNWorkflow.Controllers
{
    public class CardMixerController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CardMixerController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult CardMixer()
        {
            // Leer el idioma desde las cookies
            var lang = Request.Cookies["LanguagePreference"] ?? "en"; // Por defecto, inglés

            ViewBag.MenuItems = new List<dynamic>
            {
                new { Name = "Equipo", SubItems = new List<dynamic> { new { Name =  "Armas Cuerpo a Cuerpo",            Type = "Item_CloseCombatWeapon" },
                                                                      new { Name =  "Armas Arrojadizas / a Distancia",  Type = "Item_RangedWeapon"},
                                                                      new { Name =  "Armaduras",                        Type = "Item_Armor"},
                                                                      new { Name =  "Escudos",                          Type = "Item_Shield"},
                                                                    } 
                },
                new { Name = "Objetos Comunes", Type = "Item_CommonObject", SubItems = new List<dynamic>() },
                new { Name = "Objetos Especiales",  Type = "Item_SpecialObject", SubItems = new List<dynamic>() },
                new { Name = "Habilidades", SubItems = new List<dynamic> { 
                                                                           new { Name =  "Académicas",       Type = "Skill_Academic"},
                                                                           new { Name =  "Naturales",        Type = "Skill_Natural"},
                                                                           new { Name =  "Combate",          Type = "Skill_Combat"},
                                                                           new { Name =  "Exploración",      Type = "Skill_Exploration"},
                                                                    }
                },
                /*new { Name = "Eventos", SubItems = new List<dynamic> {
                                                                           new { Name =  "Castillo",  Type = "Events_Castle"},
                                                                           new { Name =  "Ciudad",    Type = "Events_City"},
                                                                           new { Name =  "Peligro",   Type = "Events_Danger"},
                                                                           new { Name =  "Desierto",  Type = "Events_Desert"},
                                                                           new { Name =  "Bosque",    Type = "Events_Forest"},
                                                                           new { Name =  "Glaciar",   Type = "Events_Glacier"},
                                                                           new { Name =  "Pantano",   Type = "Events_Marsh"},
                                                                           new { Name =  "Montaña",   Type = "Events_Mountain"},
                                                                           new { Name =  "Llanuras",  Type = "Events_Plains"},
                                                                           new { Name =  "Río",       Type = "Events_River"},
                                                                           new { Name =  "Camino",    Type = "Events_Road"},
                                                                           new { Name =  "Mar",       Type = "Events_Sea"},
                                                                           new { Name =  "Aldea",     Type = "Events_Village"},
                                                                           new { Name =  "Volcán",    Type = "Events_Volcano"},
            
                                                                    }
                }*/
            };



            return View();
        }


        [HttpGet]
        public IActionResult LoadCards(string type)
        {
            // Seleccionar el archivo JSON según el idioma
            string jsonFileName = $"{type}.json" ;

            // Cargar el archivo JSON y mostrar la primera tarjeta
            string jsonFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "json", jsonFileName);
            string jsonData = System.IO.File.ReadAllText(jsonFilePath);
            var cardDataArray = JsonConvert.DeserializeObject<BaseCard[]>(jsonData);
            return PartialView("_CardMixerPartial", cardDataArray);
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
            return PartialView("_CardMixerPartial", cardData);
        }
    }
}
