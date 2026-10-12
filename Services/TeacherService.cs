using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Data;
using SavelevCrossPlatform.Dtos;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Services;

public class TeacherService(LabDbContext context)
{
    public async Task<TeacherDto[]> GetAllAsync(CancellationToken cancellationToken) =>
        (await context.Teachers.AsNoTracking().ToArrayAsync(cancellationToken))
            .Select(item => item.ToDto()).ToArray();

    public async Task<TeacherDto?> GetAsync(int id, CancellationToken cancellationToken) =>
        (await context.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken))?.ToDto();

    public async Task<TeacherDto> CreateAsync(TeacherDto request, CancellationToken cancellationToken)
    {
        var teacher = new Teacher(0, request.Name, request.Discipline);
        context.Teachers.Add(teacher);
        await context.SaveChangesAsync(cancellationToken);
        return teacher.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, TeacherDto request, CancellationToken cancellationToken)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (teacher is null) return false;
        teacher.Update(request.Name, request.Discipline);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<DeleteResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (teacher is null) return DeleteResult.NotFound;
        if (await context.Reports.AnyAsync(item => item.TeacherId == id, cancellationToken))
            return DeleteResult.InUse;
        context.Teachers.Remove(teacher);
        await context.SaveChangesAsync(cancellationToken);
        return DeleteResult.Deleted;
    }
}
