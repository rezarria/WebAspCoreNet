using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models;

public class Tep
{
    public Tep()
    {
        QuyenHanTaiKhoan = new HashSet<TepTaiKhoan>();
    }

    [Key]
    public long IdTep { get; set; }
    public long IdOwner { get; set; }
    [Required]
    public string DuongDan { get; set; } = String.Empty;

    [ForeignKey(nameof(IdOwner))]
    [InverseProperty(nameof(TaiKhoan.DanhSachTep))]
    public virtual TaiKhoan TaiKhoanSoHuu { get; set; } = null!;
    [InverseProperty(nameof(TepTaiKhoan.Tep))]
    public virtual ICollection<TepTaiKhoan> QuyenHanTaiKhoan { get; set; }
}