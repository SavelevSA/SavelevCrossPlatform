using System.ComponentModel.DataAnnotations;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Dtos;

public class TeacherDto
{
    public int Id { get; private set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Discipline { get; set; } = string.Empty;

    public TeacherDto() { }

    public TeacherDto(Teacher teacher)
    {
        Id = teacher.Id;
        Name = teacher.Name;
        Discipline = teacher.Discipline;
    }
}

public class LabWorkDto
{
    public int Id { get; private set; }

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

    public LabWorkDto() { }

    public LabWorkDto(LabWork work)
    {
        Id = work.Id;
        Title = work.Title;
        Instructions = work.Instructions;
        Discipline = work.Discipline;
        RequiredEquipment = work.RequiredEquipment;
        MaxDeviationPercent = work.MaxDeviationPercent;
    }
}

public class ClassroomDto
{
    public int Id { get; private set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(30)]
    public string Number { get; set; } = string.Empty;

    [Required]
    public List<string> Equipment { get; set; } = [];

    public ClassroomDto() { }

    public ClassroomDto(Classroom classroom)
    {
        Id = classroom.Id;
        Number = classroom.Number;
        Equipment = [.. classroom.Equipment];
    }
}

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

public record ReportDetailDto(
    int Id, string Teacher, string Work, string Classroom, decimal DeviationPercent,
    bool Passed, string Conclusion, DateTimeOffset CheckedAt);

public record TeacherWorkDto(string Teacher, string Work, int Id);

public record ClassroomWorkDto(string Classroom, string Work, int Id);

public static class DtoMapper
{
    public static TeacherDto ToDto(this Teacher teacher) =>
        new(teacher);

    public static LabWorkDto ToDto(this LabWork work) =>
        new(work);

    public static ClassroomDto ToDto(this Classroom classroom) =>
        new(classroom);

    public static ReportDto ToDto(this Report report) =>
        new(report);
}
