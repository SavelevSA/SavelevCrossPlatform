using System.ComponentModel.DataAnnotations;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Dtos;

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
