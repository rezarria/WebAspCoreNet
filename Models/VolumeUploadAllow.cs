using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models;

public partial class VolumeUploadAllow
{
    [Key]
    [Column("ID_Volume__UploadAllow")]
    public long IdVolumeUploadAllow { get; set; }
    [Column("ID_Volume")]
    public long IdVolume { get; set; }
    [Required]
    public string Value { get; set; } = null!;

    [ForeignKey(nameof(IdVolume))]
    [InverseProperty(nameof(Models.Volume.VolumeUploadAllows))]
    public virtual Volume Volume { get; set; } = null!;
}