using System.ComponentModel.DataAnnotations;

namespace SporSalonuTakip.Models
{
    public class Member
    {
        public int Id { get; set; } // Birincil Anahtar

        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "En fazla 50 karakter girebilirsiniz.")]
        public string FullName { get; set; }

        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Üyelik Tipi")]
        [Required]
        public string PlanType { get; set; } // Örn: Aylık, 3 Aylık

        [Display(Name = "Ücret")]
        public decimal Price { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Aktif Üye mi?")]
        public bool IsActive { get; set; }
    }
}