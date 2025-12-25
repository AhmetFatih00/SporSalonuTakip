using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonuTakip.Data;
using SporSalonuTakip.Models;

namespace SporSalonuTakip.Controllers
{
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. LİSTELEME SAYFASI (SADECE ADMİN GİREBİLİR)
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            return View(await _context.Members.ToListAsync());
        }

        // 2. YENİ: PAKETLER SAYFASI (HERKES GİREBİLİR)
        public IActionResult Plans()
        {
            return View();
        }

        // 3. EKLEME SAYFASI - GET (HERKES GİREBİLİR)
        // Paket seçilince buraya veri gelir
        public IActionResult Create(string plan, decimal price)
        {
            // Eğer paket seçilmeden direkt linkle gelindiyse varsayılan değer ata
            if (string.IsNullOrEmpty(plan))
            {
                plan = "Aylık Standart";
                price = 3000;
            }

            var member = new Member
            {
                PlanType = plan,
                Price = price,
                StartDate = DateTime.Now,
                IsActive = true
            };

            return View(member);
        }

        // 4. EKLEME İŞLEMİ - POST (KAYDET BUTONUNA BASINCA)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,PhoneNumber,PlanType,Price,StartDate,IsActive")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                // Kayıt başarılı olunca Anasayfaya dön
                return RedirectToAction("Index", "Home");
            }
            return View(member);
        }

        // 5. DÜZENLEME SAYFASI (SADECE ADMİN)
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");

            if (id == null) return NotFound();

            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,PhoneNumber,PlanType,Price,StartDate,IsActive")] Member member)
        {
            if (id != member.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // 6. SİLME SAYFASI (SADECE ADMİN)
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");

            if (id == null) return NotFound();

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();

            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 7. DETAY SAYFASI (SADECE ADMİN)
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");

            if (id == null) return NotFound();

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();

            return View(member);
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}