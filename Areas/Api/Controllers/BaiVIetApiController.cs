using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Api.Controllers;

[Area("Api")]
[Route("/Api/[controller]")]
[ApiController]
public class BaiVietController : ControllerBase
{
    private readonly Contexts.WEBDbContext _database;
    public BaiVietController(Contexts.WEBDbContext database)
    {
        _database = database;
    }

    [HttpGet]
    public async Task<IActionResult> Get(long id)
    {
        var baiViet = await _database.BaiViet.FindAsync(id);
        if (baiViet is not null)
            return new ObjectResult(baiViet);
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Models.BaiViet baiViet)
    {
        try
        {
            if (ModelState.IsValid)
            {
                baiViet.ThoiGianTao = DateTime.Now;
                baiViet.DaDuyet = baiViet.DaDang = false;
                _database.Add(baiViet);
                await _database.SaveChangesAsync();
                return Ok(baiViet);
            }
        }
        catch (Exception e)
        {
            return new ObjectResult(e);
        }

        return UnprocessableEntity(ModelState);
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] JsonPatchDocument<Models.BaiViet> patch, long id)
    {
        if (patch is not null)
        {
            var baiViet = _database.BaiViet.Find(id);
            if (baiViet is null) return NotFound();

            patch.ApplyTo(baiViet, ModelState);
            if (ModelState.IsValid)
            {
                await _database.SaveChangesAsync();
                return new ObjectResult(baiViet);
            }
        }
        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(long id)
    {
        var baiViet = _database.BaiViet.Find(id);

        if (baiViet is null)
            return NotFound();
        _database.Remove(baiViet);
        await _database.SaveChangesAsync();
        return NoContent();
    }

    [HttpHead]
    public string Test() => "OK";
}