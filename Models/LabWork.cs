namespace SavelevCrossPlatform.Models;

public class LabWork
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Instructions { get; private set; } = string.Empty;
    public string Discipline { get; private set; } = string.Empty;
    public string RequiredEquipment { get; private set; } = string.Empty;
    public decimal MaxDeviationPercent { get; private set; }

    private LabWork() { }

    public LabWork(int id, string title, string instructions, string discipline, string requiredEquipment, decimal maxDeviationPercent)
    {
        Id = id;
        Update(title, instructions, discipline, requiredEquipment, maxDeviationPercent);
    }

    public void Update(string title, string instructions, string discipline, string requiredEquipment, decimal maxDeviationPercent)
    {
        Title = title.Trim();
        Instructions = instructions.Trim();
        Discipline = discipline.Trim();
        RequiredEquipment = requiredEquipment.Trim();
        MaxDeviationPercent = maxDeviationPercent;
    }

    public bool IsWithinTolerance(decimal deviationPercent) =>
        deviationPercent >= 0 && deviationPercent <= MaxDeviationPercent;
}
