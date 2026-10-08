using System.ComponentModel.DataAnnotations;

namespace SavelevCrossPlatform;

public class TeacherRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Discipline { get; set; } = string.Empty;
}

public class LabWorkRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string Instructions { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Discipline { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string RequiredEquipment { get; set; } = string.Empty;
    [Range(typeof(decimal), "0", "100")]
    public decimal MaxDeviationPercent { get; set; }
}

public class ClassroomRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(30)]
    public string Number { get; set; } = string.Empty;
    [Required]
    public string[] Equipment { get; set; } = [];
}

public class ReportRequest
{
    [Range(1, int.MaxValue)]
    public int TeacherId { get; set; }
    [Range(1, int.MaxValue)]
    public int LabWorkId { get; set; }
    [Range(1, int.MaxValue)]
    public int ClassroomId { get; set; }
    [Range(typeof(decimal), "0", "100")]
    public decimal DeviationPercent { get; set; }
}
