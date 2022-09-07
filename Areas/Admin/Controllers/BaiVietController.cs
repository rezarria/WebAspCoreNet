using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
public class BaiViet : Controller
{
    private readonly Contexts.WEBDbContext _database;
    public BaiViet(Contexts.WEBDbContext database)
    {
        _database = database;
    }

    public IActionResult DanhSach()
    {
        return View(_database.BaiViet.AsNoTracking().ToArray());
    }
}