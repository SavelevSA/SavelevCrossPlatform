using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Data;
using SavelevCrossPlatform.Dtos;

namespace SavelevCrossPlatform.Services;

public class QueryService(LabDbContext context)
{
    public async Task<List<ReportDetailDto>> ReportDetailsAsync(bool? passed, CancellationToken cancellationToken)
    {
        var reports = await context.Reports.AsNoTracking()
            .Include(report => report.Teacher)
            .Include(report => report.LabWork)
            .Include(report => report.Classroom)
            .Where(report => !passed.HasValue || report.Passed == passed.Value)
            .ToListAsync(cancellationToken);

        return reports
            .Select(report => new ReportDetailDto(
                report.Id, report.Teacher.Name, report.LabWork.Title,
                report.Classroom.Number, report.DeviationPercent,
                report.Passed, report.Conclusion, report.CheckedAt))
            .ToList();
    }

    public async Task<List<TeacherWorkDto>?> TeacherWorksAsync(int teacherId, CancellationToken cancellationToken)
    {
        var teacher = await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == teacherId, cancellationToken);
        if (teacher is null) return null;
        var works = await context.Works.AsNoTracking().ToListAsync(cancellationToken);
        return works.Where(teacher.CanSupervise)
            .Select(work => new TeacherWorkDto(teacher.Name, work.Title, work.Id))
            .ToList();
    }

    public async Task<List<ClassroomWorkDto>?> ClassroomWorksAsync(int classroomId, CancellationToken cancellationToken)
    {
        var classroom = await context.Classrooms.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == classroomId, cancellationToken);
        if (classroom is null) return null;
        var works = await context.Works.AsNoTracking().ToListAsync(cancellationToken);
        return works.Where(classroom.CanHost)
            .Select(work => new ClassroomWorkDto(classroom.Number, work.Title, work.Id))
            .ToList();
    }
}
