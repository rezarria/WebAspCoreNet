using elFinder.Net.AspNetCore.Extensions;
using elFinder.Net.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Contexts;

namespace Web.Areas.Api.Controllers;
public partial class FileController : Controller
{
    private readonly IConnector _connector;
    private readonly IDriver _driver;
    private readonly WEBDbContext _database;
    private readonly DirectoryInfo _rootDirectory;
    private readonly DirectoryInfo _tempDirectory;

    public FileController(IConnector connector, IDriver driver, WEBDbContext database)
    {
        _connector = connector;
        _driver = driver;
        _database = database;
        _rootDirectory = new(".storage");
        _tempDirectory = new(".temp");
    }

    private async Task Setup()
    {
        // HttpContext.RequestAborted.ThrowIfCancellationRequested();

        // Volume volume = new(driver: _driver,
        //     rootDirectory: Path.Combine(_rootDirectory.FullName, "Disk_0"),
        //     tempDirectory: Path.Combine(_tempDirectory.FullName, "Disk_0"),
        //     url: "/api/file/storage/Disk_0/",
        //     thumbUrl: "/api/file/thumb/",
        //     thumbnailDirectory: Path.Combine(_tempDirectory.FullName, "thumb", "Disk_0"))
        // {
        //     Name = "disk 0",
        //     MaxUploadConnections = 200,
        // };

        // _connector.AddVolume(volume);
        // await _driver.SetupVolumeAsync(volume, HttpContext.RequestAborted);

        ThietLapVolumeTheoDatabase();
        await Task.CompletedTask;
    }

    /// <summary>
    /// Nếu đường dẫn chưa tồn tại thì tạo
    /// </summary>
    /// <param name="path">Đường dẫn</param>
    /// <returns>Trả về <c>DirectoryInfo</c> về đường đẫn đó</returns>
    private DirectoryInfo IsNotExistThenCreateDirectory(string path)
    {
        DirectoryInfo directoryInfo = new(path);
        if (!directoryInfo.Exists)
        {
            Directory.CreateDirectory(directoryInfo.FullName);
        }
        return directoryInfo;
    }

    /// <summary>
    /// Dựa vào cơ sở dữ liệu để tạo ổ đĩa
    /// </summary>
    private async void ThietLapVolumeTheoDatabase()
    {
        Web.Models.Volume[] volumeInfos = _database.Volume.Include(x => x.VolumeUploadAllows)
            .Include(x => x.VolumeUploadDenies).AsNoTracking().ToArray();
        foreach (Models.Volume volumeInfo in volumeInfos)
        {
            DirectoryInfo rootPath = IsNotExistThenCreateDirectory(volumeInfo.RootDirectory);
            DirectoryInfo tempPath = IsNotExistThenCreateDirectory(volumeInfo.TempDirectory);
            DirectoryInfo thumbnailPath = IsNotExistThenCreateDirectory(volumeInfo.ThumbnailDirectory);

            Volume volume = new(driver: _driver,
                rootDirectory: rootPath.FullName,
                tempDirectory: tempPath.FullName,
                thumbnailDirectory: thumbnailPath.FullName,
                url: $"/api/file/storage/{volumeInfo.Name.Trim().Replace(' ', '_')}",
                thumbUrl: $"/api/file/thumb/")
            {
                Name = volumeInfo.Name,
                MaxUploadConnections = volumeInfo.MaxUploadConnections,
                IsLocked = volumeInfo.IsLocked,
                IsReadOnly = volumeInfo.IsReadOnly,
                IsShowOnly = volumeInfo.IsShowOnly,
                CopyOverwrite = volumeInfo.CopyOverwrite,
                ThumbnailSize = volumeInfo.ThumbnailSize,
                UploadOverwrite = volumeInfo.UploadOverwrite
            };

            if (volumeInfo.MaxUploadFiles.HasValue)
            {
                volume.MaxUploadFiles = volumeInfo.MaxUploadFiles;
            }

            if (volumeInfo.MaxUploadSize.HasValue)
            {
                volume.MaxUploadSize = volumeInfo.MaxUploadSize;
            }

            if (volumeInfo.VolumeUploadAllows.Any())
            {
                volume.UploadAllow = volumeInfo.VolumeUploadAllows.Select(x => x.Value);
            }

            if (volumeInfo.VolumeUploadDenies.Any())
            {
                volume.UploadDeny = volumeInfo.VolumeUploadDenies.Select(x => x.Value);
            }

            _connector.AddVolume(volume);
            await _driver.SetupVolumeAsync(volume, HttpContext.RequestAborted);

            ThietLapPhanQuyenVolume(volume);
        }
    }

    /// <summary>
    /// Thiết lập chi tiết ổ đĩa theo cơ sở dữ liệu
    /// </summary>
    /// <param name="volume"></param>
    /// <returns></returns>
    private async void ThietLapPhanQuyenVolume(Volume volume)
    {
        string idTaiKhoan = User.Claims.Single(claim => claim.Type == "IdTaiKhoan").Value;
        IQueryable<Models.TepTaiKhoan> UsersFileList = _database.TepTaiKhoan
            .Where(x => x.IdTaiKhoan.ToString() == idTaiKhoan)
            .Include(x => x.Tep)
            .AsNoTracking();

        List<FilteredObjectAttribute> FilteredObjectAttributeList = new()
            {
                //Khóa hết
                new()
                {
                    Write = false,
                    Read = false,
                    Locked = false,
                    Access = false,
                    ShowOnly = false,
                    Visible = false,
                    ObjectFilter = (obj) =>
                        obj.FullName != _rootDirectory.FullName && obj.FullName.StartsWith(_rootDirectory.FullName)
                }
            };


        ICollection<Models.Tep> FileCfgList = _database.TaiKhoan.Where(x => x.Id.ToString() == idTaiKhoan)
                                                                .Include(x => x.DanhSachTep)
                                                                .AsNoTracking()
                                                                .First().DanhSachTep;

        foreach (Models.Tep tep in FileCfgList)
        {
            string path = Path.Combine(_rootDirectory.FullName, tep.DuongDan);
            FilteredObjectAttributeList.Add(new()
            {
                Write = true,
                Read = true,
                Locked = false,
                Access = true,
                ShowOnly = false,
                Visible = true,
                ObjectFilter = obj => obj.FullName.StartsWith(path)
            });
        }


        await UsersFileList.ForEachAsync(item =>
        {
            string path = Path.Combine(_rootDirectory.FullName, item.Tep.DuongDan);
            FilteredObjectAttributeList.Add(new()
            {
                Write = item.Write,
                Read = item.Read,
                Locked = item.Locked,
                Access = item.Access,
                ShowOnly = item.ShowOnly,
                Visible = item.Visible,
                ObjectFilter = obj => obj.FullName == path || obj.FullName.StartsWith(path)
            });
        }, HttpContext.RequestAborted);


        volume.ObjectAttributes = FilteredObjectAttributeList;
    }
}