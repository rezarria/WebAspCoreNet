using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Web.ETC;

namespace Web.Areas.Api.Controllers;

[ApiController]
[Route("/api/taikhoan")]
public class TaiKhoanController : ControllerBase
{
    private readonly Contexts.WEBDbContext _database;
    public TaiKhoanController(Contexts.WEBDbContext database)
    {
        _database = database;
    }

    [HttpGet]
    public async Task<IActionResult> Lay(long id)
    {
        var taiKhoan = _database.TaiKhoan.Find(id);
        if (taiKhoan is null)
            return NotFound();
        return new ObjectResult(taiKhoan);
    }

    [HttpPost]
    public async Task<IActionResult> TaoTaiKhoan([FromBody] Models.TaiKhoan taiKhoan)
    {
        if (!_database.TaiKhoan.Any(x => x.Username == taiKhoan.Username))
        {
            using (var rng = RandomNumberGenerator.Create())
                taiKhoan.MatKhau = Convert.ToBase64String(MatKhau.Hash(taiKhoan.MatKhau, rng));
            _database.Add(taiKhoan);
            await _database.SaveChangesAsync();
            return Ok();
        }
        return BadRequest();
    }
    [HttpDelete]
    public async Task<IActionResult> Xoa(long id)
    {
        var taiKhoan = _database.TaiKhoan.Find(id);
        if (taiKhoan is not null)
        {
            _database.Remove(taiKhoan);
            await _database.SaveChangesAsync();
            return NoContent();
        }
        return NotFound();
    }

    [HttpPatch]
    public async Task<IActionResult> CapNhat([FromBody] JsonPatchDocument<Models.TaiKhoan> patch, long id)
    {
        Console.WriteLine(patch);
        var taiKhoan = _database.TaiKhoan.Find(id);
        if (taiKhoan is not null)
        {
            patch.ApplyTo(taiKhoan, ModelState);
        }
        return Ok();
    }
}