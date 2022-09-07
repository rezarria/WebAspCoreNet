using System.Globalization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Web.ETC;

namespace Web.Areas.Api.Controllers;

public class TaiKhoanActionController : ControllerBase
{
    private readonly Contexts.WEBDbContext _database;
    public TaiKhoanActionController(Contexts.WEBDbContext database)
    {
        _database = database;
    }

    [HttpPost]
    [HttpGet]
    [Route("/api/taikhoan/dangnhap")]
    public async Task<IActionResult> DangNhap([FromBody][Bind($"{nameof(Models.TaiKhoan.Username)}{nameof(Models.TaiKhoan.MatKhau)}")] Models.TaiKhoan input, string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated ?? false)
            return NoContent();

        try
        {
            var taiKhoan = _database.TaiKhoan.Single(x => x.Username == input.Username);
            if (await TaiKhoan.DangNhapBangMatKhau(HttpContext, taiKhoan, input.MatKhau))
            {
                returnUrl ??= "/";
                return Redirect(returnUrl);
            }
            return BadRequest();
        }
        catch (System.Exception)
        {
            return BadRequest();
        }
    }
}