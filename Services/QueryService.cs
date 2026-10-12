using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Data;
using SavelevCrossPlatform.Dtos;

namespace SavelevCrossPlatform.Services;

public class QueryService(LabDbContext context)
{
    public async Task<List<ReportDetailDto>> ReportDetailsAsync(bool? passed, CancellationToken cancellationToken) =>
        await (
            from report in context.Reports.AsNoTracking()
            join teacher in context.Teachers.AsNoTracking() on report.TeacherId equals teacher.Id
            join work in context.Works.AsNoTracking() on report.LabWorkId equals work.Id
            join classroom in context.Classrooms.AsNoTracking() on report.ClassroomId equals classroom.Id
            where !passed.HasValue || report.Passed == passed.Value
            select new ReportDetailDto(
                report.Id, teacher.Name, work.Title, classroom.Number,
                report.DeviationPercent, report.Passed, report.Conclusion, report.CheckedAt))
        .ToListAsync(cancellationToken);

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
