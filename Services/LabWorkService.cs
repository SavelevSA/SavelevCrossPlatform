using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Data;
using SavelevCrossPlatform.Dtos;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Services;

public class LabWorkService(LabDbContext context)
{
    public async Task<LabWorkDto[]> GetAllAsync(CancellationToken cancellationToken) =>
        (await context.Works.AsNoTracking().ToArrayAsync(cancellationToken))
            .Select(item => item.ToDto()).ToArray();

    public async Task<LabWorkDto?> GetAsync(int id, CancellationToken cancellationToken) =>
        (await context.Works.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken))?.ToDto();

    public async Task<LabWorkDto> CreateAsync(LabWorkDto request, CancellationToken cancellationToken)
    {
        var work = new LabWork(0, request.Title, request.Instructions, request.Discipline,
            request.RequiredEquipment, request.MaxDeviationPercent);
        context.Works.Add(work);
        await context.SaveChangesAsync(cancellationToken);
        return work.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, LabWorkDto request, CancellationToken cancellationToken)
    {
        var work = await context.Works.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (work is null) return false;
        work.Update(request.Title, request.Instructions, request.Discipline,
            request.RequiredEquipment, request.MaxDeviationPercent);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<DeleteResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var work = await context.Works.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (work is null) return DeleteResult.NotFound;
        if (await context.Reports.AnyAsync(item => item.LabWorkId == id, cancellationToken))
            return DeleteResult.InUse;
        context.Works.Remove(work);
        await context.SaveChangesAsync(cancellationToken);
        return DeleteResult.Deleted;
    }
}
