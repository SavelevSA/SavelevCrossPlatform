using System.ComponentModel.DataAnnotations;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Dtos;

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
