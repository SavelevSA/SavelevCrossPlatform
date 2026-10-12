using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Dtos;

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
