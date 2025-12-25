using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Session için gerekli

namespace SporSalonuTakip.Controllers
{
    public class AccountController : Controller
    {
        // 1. Giriş Sayfasını Göster
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 2. Giriş Yapılınca Çalışan Kısım (POST)
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Şifre Kontrolü (Küçük-büyük harf duyarlı olmasın diye username'i küçültüyoruz)
            // Kullanıcı Adı: admin
            // Şifre: 123
            if (username != null && password != null && 
                username.ToLower() == "admin" && password == "123")
            {
                // Giriş Başarılı!
                // Oturuma "Admin" olduğunu kaydedelim
                HttpContext.Session.SetString("UserRole", "Admin");

                // Yönetim Paneline yönlendir
                return RedirectToAction("Index", "Members");
            }
            else
            {
                // Hatalı Giriş
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
                return View();
            }
        }

        // 3. Çıkış Yap
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Oturumu temizle
            return RedirectToAction("Index", "Home"); // Anasayfaya dön
        }
    }
}