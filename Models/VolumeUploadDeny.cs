using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models;

public partial class VolumeUploadDeny
{
    [Key]
    [Column("ID_Volume__UploadDeny")]
    public long IdVolumeUploadDeny { get; set; }
    [Column("ID_Volume")]
    public long IdVolume { get; set; }
    [Required]
    public string Value { get; set; }  = String.Empty;

    [ForeignKey(nameof(IdVolume))]
    [InverseProperty(nameof(Models.Volume.VolumeUploadDenies))]
    public virtual Volume Volume { get; set; } = null!;
}