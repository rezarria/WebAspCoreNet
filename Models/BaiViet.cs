using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class BaiViet
{
    [Key]
    public long Id { get; set; }
    [Required]
    [Display(Name ="Tiêu đề")]
    public string TieuDe { get; set; } = null!;
    [Required]
    [Display(Name ="Nội dung")]
    public string NoiDung { get; set; } = null!;
    public string? AnhBia { get; set; } = null;
    [Display(Name ="Thời gian tạo")]
    public DateTime? ThoiGianTao { get; set; }
    [Display(Name ="Lần sửa cuối")]
    public DateTime? ThoiGianLanSuaCuoi { get; set; }
    public bool DaDuyet { get; set; }
    public DateTime? ThoiGianDuyet { get; set; }
    public bool DaDang { get; set; }
    public DateTime? ThoiGianDang { get; set; }
}