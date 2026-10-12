namespace SavelevCrossPlatform.Models;

public class Classroom
{
    public int Id { get; private set; }
    public string Number { get; private set; } = string.Empty;
    public List<string> Equipment { get; private set; } = [];

    private Classroom() { }

    public Classroom(int id, string number, List<string> equipment)
    {
        Id = id;
        Update(number, equipment);
    }

    public void Update(string number, List<string> equipment)
    {
        Number = number.Trim();
        Equipment = equipment.Select(item => item.Trim()).ToList();
    }

    public bool CanHost(LabWork work) => Equipment.Contains(
        work.RequiredEquipment, StringComparer.OrdinalIgnoreCase);
}
