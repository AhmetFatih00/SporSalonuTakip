using Microsoft.AspNetCore.Mvc;

namespace SporSalonuTakip.Controllers
{
    public class ProgramsController : Controller
    {
        // 1. Ana Sayfa (Programların Listelendiği Yer)
        public IActionResult Index()
        {
            return View();
        }

        // 2. YENİ EKLEDİĞİMİZ KISIM: Hypertrophy Sayfası
        public IActionResult Hypertrophy()
        {
            return View();
        }
        // Yağ Yakımı Sayfasını Açan Metot
        public IActionResult FatLoss()
        {
            return View();
        }
        // Full Body Sayfasını Açan Metot
        public IActionResult FullBody()
        {
            return View();
        }
    }
}