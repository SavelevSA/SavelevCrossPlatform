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
