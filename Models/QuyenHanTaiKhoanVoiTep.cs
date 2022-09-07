using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Web.Models;

public partial class TepTaiKhoan
{
    [Key]
    public long IdTep { get; set; }
    [Key]
    public long IdTaiKhoan { get; set; }
    public bool Read { get; set; }
    public bool Write { get; set; }
    public bool Locked { get; set; }
    public bool Access { get; set; }
    public bool ShowOnly { get; set; }
    public bool Visible { get; set; }

    [ForeignKey(nameof(IdTaiKhoan))]
    [InverseProperty(nameof(Models.TaiKhoan.QuyenHanTep))]
    public virtual TaiKhoan TaiKhoan { get; set; }
    [ForeignKey(nameof(IdTep))]
    [InverseProperty(nameof(Models.Tep.QuyenHanTaiKhoan))]
    public virtual Tep Tep { get; set; }
}

