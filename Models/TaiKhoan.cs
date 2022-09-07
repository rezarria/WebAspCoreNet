using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Web.Models;

public class TaiKhoan
{
    [Key]
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string MatKhau { get; set; } = null!;
    public string? Avatar { get; set; }

    [InverseProperty(nameof(TepTaiKhoan.TaiKhoan))]
    public virtual ICollection<TepTaiKhoan> QuyenHanTep { get; set; } = new HashSet<TepTaiKhoan>();
    [InverseProperty(nameof(Tep.TaiKhoanSoHuu))]
    public virtual ICollection<Tep> DanhSachTep { get; set; } = new HashSet<Tep>();
}