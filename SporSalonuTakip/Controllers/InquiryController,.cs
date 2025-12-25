using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonuTakip.Data;

namespace SporSalonuTakip.Controllers
{
    public class InquiryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InquiryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Sayfa ilk açıldığında veya sorgulama yapıldığında burası çalışır
        public IActionResult Index(string phoneNumber)
        {
            // 1. Eğer numara girilmemişse sadece boş sayfayı göster
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return View();
            }

            // 2. Numarayı veritabanında ara (Boşlukları temizleyerek)
            var member = _context.Members
                .FirstOrDefault(m => m.PhoneNumber == phoneNumber.Trim());

            // 3. Üye bulunamazsa hata mesajı ver
            if (member == null)
            {
                ViewBag.Error = "Bu telefon numarasına ait bir kayıt bulunamadı.";
                return View();
            }

            // 4. Üye bulundu! Şimdi bitiş tarihini ve kalan günü hesaplayalım
            DateTime endDate = member.StartDate;

            // Paket tipine göre bitiş tarihini hesapla
            if (member.PlanType.Contains("3 Aylık"))
            {
                endDate = member.StartDate.AddMonths(3);
            }
            else if (member.PlanType.Contains("Yıllık"))
            {
                endDate = member.StartDate.AddYears(1);
            }
            else // Aylık veya diğerleri
            {
                endDate = member.StartDate.AddMonths(1);
            }

            // Kalan gün sayısı (Bitiş - Bugün)
            TimeSpan timeSpan = endDate - DateTime.Now;
            int daysLeft = timeSpan.Days;

            // Bu bilgileri sayfaya (View) taşıyalım
            ViewBag.EndDate = endDate; // Bitiş Tarihi
            ViewBag.DaysLeft = daysLeft; // Kalan Gün

            return View(member);
        }
    }
}