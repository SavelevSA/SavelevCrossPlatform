using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Data;
using SavelevCrossPlatform.Dtos;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Services;

public class ClassroomService(LabDbContext context)
{
    public async Task<ClassroomDto[]> GetAllAsync(CancellationToken cancellationToken) =>
        (await context.Classrooms.AsNoTracking().ToArrayAsync(cancellationToken))
            .Select(item => item.ToDto()).ToArray();

    public async Task<ClassroomDto?> GetAsync(int id, CancellationToken cancellationToken) =>
        (await context.Classrooms.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken))?.ToDto();

    public async Task<ClassroomDto> CreateAsync(ClassroomDto request, CancellationToken cancellationToken)
    {
        var classroom = new Classroom(0, request.Number, request.Equipment);
        context.Classrooms.Add(classroom);
        await context.SaveChangesAsync(cancellationToken);
        return classroom.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, ClassroomDto request, CancellationToken cancellationToken)
    {
        var classroom = await context.Classrooms.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (classroom is null) return false;
        classroom.Update(request.Number, request.Equipment);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<DeleteResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var classroom = await context.Classrooms.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (classroom is null) return DeleteResult.NotFound;
        if (await context.Reports.AnyAsync(item => item.ClassroomId == id, cancellationToken))
            return DeleteResult.InUse;
        context.Classrooms.Remove(classroom);
        await context.SaveChangesAsync(cancellationToken);
        return DeleteResult.Deleted;
    }
}
