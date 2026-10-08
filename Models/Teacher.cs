namespace SavelevCrossPlatform.Models;

public class Teacher
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Discipline { get; private set; } = string.Empty;

    private Teacher() { }

    public Teacher(int id, string name, string discipline)
    {
        Id = id;
        Update(name, discipline);
    }

    public void Update(string name, string discipline)
    {
        Name = name.Trim();
        Discipline = discipline.Trim();
    }

    public bool CanSupervise(LabWork work) =>
        string.Equals(Discipline, work.Discipline, StringComparison.OrdinalIgnoreCase);
}
