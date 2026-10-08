using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Data;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Route("api/lab-works")]
public class LabWorksController(LabDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LabWork>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await db.Works.AsNoTracking().ToArrayAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LabWork>> Get(int id, CancellationToken cancellationToken)
    {
        var work = await db.Works.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        return work is null ? NotFound() : Ok(work);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<LabWork>> Create(LabWorkRequest request, CancellationToken cancellationToken)
    {
        if (Invalid(request)) return BadRequest("Название, указания, дисциплина и оборудование обязательны.");
        var work = new LabWork(0, request.Title, request.Instructions, request.Discipline,
            request.RequiredEquipment, request.MaxDeviationPercent);
        db.Works.Add(work);
        await db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = work.Id }, work);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, LabWorkRequest request, CancellationToken cancellationToken)
    {
        if (Invalid(request)) return BadRequest("Название, указания, дисциплина и оборудование обязательны.");
        var work = await db.Works.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (work is null) return NotFound();
        work.Update(request.Title, request.Instructions, request.Discipline,
            request.RequiredEquipment, request.MaxDeviationPercent);
        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var work = await db.Works.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (work is null) return NotFound();
        if (await db.Reports.AnyAsync(item => item.LabWorkId == id, cancellationToken))
            return Conflict("Работа используется в отчёте.");
        db.Works.Remove(work);
        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private static bool Invalid(LabWorkRequest request) =>
        string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Instructions) ||
        string.IsNullOrWhiteSpace(request.Discipline) || string.IsNullOrWhiteSpace(request.RequiredEquipment);
}
