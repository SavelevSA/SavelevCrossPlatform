namespace SavelevCrossPlatform.Models;

public class Report
{
    public int Id { get; private set; }
    public int TeacherId { get; private set; }
    public int LabWorkId { get; private set; }
    public int ClassroomId { get; private set; }
    public decimal DeviationPercent { get; private set; }
    public bool Passed { get; private set; }
    public string Conclusion { get; private set; } = string.Empty;
    public DateTimeOffset CheckedAt { get; private set; }

    private Report() { }

    public Report(int id, Teacher teacher, LabWork work, Classroom classroom, decimal deviationPercent)
    {
        Id = id;
        Recheck(teacher, work, classroom, deviationPercent);
    }

    public void Recheck(Teacher teacher, LabWork work, Classroom classroom, decimal deviationPercent)
    {
        TeacherId = teacher.Id;
        LabWorkId = work.Id;
        ClassroomId = classroom.Id;
        DeviationPercent = deviationPercent;
        Passed = teacher.CanSupervise(work) && classroom.CanHost(work) && work.IsWithinTolerance(deviationPercent);
        Conclusion = !teacher.CanSupervise(work) ? "Профиль преподавателя не соответствует работе."
            : !classroom.CanHost(work) ? "В аудитории нет необходимого оборудования."
            : !work.IsWithinTolerance(deviationPercent) ? "Измеренное отклонение превышает допустимое."
            : "Проверка пройдена.";
        CheckedAt = DateTimeOffset.UtcNow;

    }
}
