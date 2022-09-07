using elFinder.Net.AspNetCore.Extensions;
using elFinder.Net.AspNetCore.Helper;
using elFinder.Net.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Api.Controllers;

[Area("Api")]
public partial class FileController : Controller
{
    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> Connecter()
    {
        await Setup();
        elFinder.Net.Core.Models.Command.ConnectorCommand lenh = ConnectorHelper.ParseCommand(Request);
        System.Threading.CancellationTokenSource token = ConnectorHelper.RegisterCcTokenSource(HttpContext);
        elFinder.Net.Core.Models.Result.ConnectorResult action = await _connector.ProcessAsync(lenh, token);
        IActionResult ketQua = action.ToActionResult(HttpContext);
        return ketQua;
    }

    [HttpGet]
    [HttpPost]
    [Route("[area]/[controller]/storage/{**path}")]
    public async Task<IActionResult> GetFile(string path)
    {
        await Setup();
        string fullPath = Path.Combine(_rootDirectory.FullName, path);
        return await this.GetPhysicalFileAsync(_connector, fullPath, HttpContext.RequestAborted);
    }

    [HttpGet]
    [HttpPost]
    [Route("[area]/[controller]/thumb/{**mucTieu}")]
    public async Task<IActionResult> Thumb(string mucTieu)
    {
        await Setup();
        try
        {
            elFinder.Net.Core.Services.Drawing.ImageWithMimeType thumb =
                await _connector.GetThumbAsync(mucTieu, HttpContext.RequestAborted);
            IActionResult ketQua = ConnectorHelper.GetThumbResult(thumb);
            return ketQua;
        }
        catch (System.Exception)
        {
            return StatusCode(404);
        }
    }
}