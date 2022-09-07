using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
public class TaiKhoanController : Controller
{
    private Contexts.WEBDbContext _database;
    public TaiKhoanController(Contexts.WEBDbContext database)
    {
        _database = database;
    }

    public IActionResult DangNhap()
    {
        return View();
    }

    public IActionResult DanhSach()
    {
        var danhSach = _database.TaiKhoan.Select(x => new ModelDTO(x)).ToArray();
        return View(danhSach);
    }

    public class ModelDTO
    {
        public long? Id { get; set; }
        public string? Username { get; set; }
        public string? MatKhau { get; set; } = null!;
        public string? Avatar { get; set; }
        public ModelDTO(Models.TaiKhoan taiKhoan)
        {
            Id = taiKhoan.Id;
            Username = taiKhoan.Username;
            Avatar = taiKhoan.Avatar;
        }
    }

    public class Model
    {
        [EmailAddress(ErrorMessage = "xxxx")]
        [Required(ErrorMessage = "Phải nhập")]
        public string Username { get; set; } = String.Empty;
        [PasswordPropertyTextAttribute]
        public string MatKhau { get; set; } = String.Empty;
    }


}