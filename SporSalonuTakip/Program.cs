using Microsoft.EntityFrameworkCore;
using SporSalonuTakip.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVÝSLERÝ EKLEME BÖLÜMÜ
// -------------------------------------------------------------------------
builder.Services.AddControllersWithViews();

// Oturum (Session) Açma Servisi
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Veritabaný Baðlantýsý (PostgreSQL)
// Render.com için Npgsql ayarý
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
// -------------------------------------------------------------------------

var app = builder.Build();

// 2. UYGULAMA AYARLARI (PIPELINE)
// -------------------------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Oturumu Aktif Et
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --- OTOMATÝK MIGRATION (Veritabanýný Kur) ---
// Bu kod Render'da site ilk açýldýðýnda tablolarý otomatik oluþturur.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabaný oluþturulurken bir hata meydana geldi.");
    }
}
// ---------------------------------------------

app.Run();