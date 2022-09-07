using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models;
public partial class Volume
{
    public Volume()
    {
        VolumeUploadAllows = new HashSet<VolumeUploadAllow>();
        VolumeUploadDenies = new HashSet<VolumeUploadDeny>();
    }

    [Key]
    [Column("ID_Volume")]
    public long IdVolume { get; set; }
    public bool UploadOverwrite { get; set; }
    public bool CopyOverwrite { get; set; }
    public bool IsShowOnly { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsLocked { get; set; }
    public int? MaxUploadFiles { get; set; }
    public int MaxUploadConnections { get; set; }
    public double? MaxUploadSize { get; set; }

    [Required]
    [StringLength(1)]
    public string DirectorySeparatorChar { get; set; } = null!;
    public int ThumbnailSize { get; set; }
    [Required]
    public string StartDirectory { get; set; } = null!;
    [Required]
    public string ThumbUrl { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string VolumeId { get; set; } = null!;
    [Required]
    public string RootDirectory { get; set; } = null!;
    [Required]
    public string TempDirectory { get; set; } = null!;
    [Required]
    public string ThumbnailDirectory { get; set; } = null!;

    [InverseProperty(nameof(VolumeUploadAllow.Volume))]
    public virtual ICollection<VolumeUploadAllow> VolumeUploadAllows { get; set; }
    [InverseProperty(nameof(VolumeUploadDeny.Volume))]
    public virtual ICollection<VolumeUploadDeny> VolumeUploadDenies { get; set; }
}