using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication;

namespace Web.ETC;

public static class TaiKhoan
{
    public static async Task<bool> DangNhapBangMatKhau(HttpContext context, Models.TaiKhoan taiKhoan, string matKhau)
    {
        var matKhauMaHoa = Convert.FromBase64String(taiKhoan.MatKhau);
        var iterConunt = default(int);
        KeyDerivationPrf prf;
        var ketQua = MatKhau.XacThuc(matKhauMaHoa, matKhau, out iterConunt, out prf);
        if (ketQua)
        {
            var claims = new Claim[] { };
            var claimIdentity = new ClaimsIdentity(claims, "Cookie.Auth");
            var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
            await context.SignInAsync(claimsPrincipal);
            return true;
        }
        return false;
    }
}