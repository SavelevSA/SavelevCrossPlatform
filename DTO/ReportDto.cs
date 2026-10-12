using System.ComponentModel.DataAnnotations;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Dtos;

public class ReportDto
{
    public int Id { get; private set; }

    [Range(1, int.MaxValue)]
    public int TeacherId { get; set; }

    [Range(1, int.MaxValue)]
    public int LabWorkId { get; set; }

    [Range(1, int.MaxValue)]
    public int ClassroomId { get; set; }

    [Range(typeof(decimal), "0", "100")]
    public decimal DeviationPercent { get; set; }

    public bool Passed { get; private set; }
    public string Conclusion { get; private set; } = string.Empty;
    public DateTimeOffset CheckedAt { get; private set; }

    public ReportDto() { }

    public ReportDto(Report report)
    {
        Id = report.Id;
        TeacherId = report.TeacherId;
        LabWorkId = report.LabWorkId;
        ClassroomId = report.ClassroomId;
        DeviationPercent = report.DeviationPercent;
        Passed = report.Passed;
        Conclusion = report.Conclusion;
        CheckedAt = report.CheckedAt;
    }
}
