using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Data;
using SavelevCrossPlatform.Dtos;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Services;

public class ReportService(LabDbContext context)
{
    public async Task<List<ReportDto>> GetAllAsync(CancellationToken cancellationToken) =>
        (await context.Reports.AsNoTracking().ToListAsync(cancellationToken))
            .Select(item => item.ToDto()).ToList();

    public async Task<ReportDto?> GetAsync(int id, CancellationToken cancellationToken) =>
        (await context.Reports.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken))?.ToDto();

    public async Task<ReportDto?> CreateAsync(ReportDto request, CancellationToken cancellationToken)
    {
        var (teacher, work, classroom) = await ResolveAsync(request, cancellationToken);
        if (teacher is null || work is null || classroom is null) return null;
        var report = new Report(0, teacher, work, classroom, request.DeviationPercent);
        context.Reports.Add(report);
        await context.SaveChangesAsync(cancellationToken);
        return report.ToDto();
    }

    public async Task<ReportUpdateResult> UpdateAsync(int id, ReportDto request, CancellationToken cancellationToken)
    {
        var report = await context.Reports.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (report is null) return ReportUpdateResult.NotFound;
        var (teacher, work, classroom) = await ResolveAsync(request, cancellationToken);
        if (teacher is null || work is null || classroom is null)
            return ReportUpdateResult.InvalidReferences;
        report.Recheck(teacher, work, classroom, request.DeviationPercent);
        await context.SaveChangesAsync(cancellationToken);
        return ReportUpdateResult.Updated;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var report = await context.Reports.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (report is null) return false;
        context.Reports.Remove(report);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task<(Teacher?, LabWork?, Classroom?)> ResolveAsync(
        ReportDto request, CancellationToken cancellationToken)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(
            item => item.Id == request.TeacherId, cancellationToken);
        var work = await context.Works.FirstOrDefaultAsync(
            item => item.Id == request.LabWorkId, cancellationToken);
        var classroom = await context.Classrooms.FirstOrDefaultAsync(
            item => item.Id == request.ClassroomId, cancellationToken);
        return (teacher, work, classroom);
    }
}
