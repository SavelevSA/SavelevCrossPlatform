using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Data;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Route("api/classrooms")]
public class ClassroomsController(LabDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Classroom>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await db.Classrooms.AsNoTracking().ToArrayAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Classroom>> Get(int id, CancellationToken cancellationToken)
    {
        var classroom = await db.Classrooms.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        return classroom is null ? NotFound() : Ok(classroom);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Classroom>> Create(ClassroomRequest request, CancellationToken cancellationToken)
    {
        if (Invalid(request)) return BadRequest("Номер аудитории и список оборудования обязательны.");
        var classroom = new Classroom(0, request.Number, request.Equipment);
        db.Classrooms.Add(classroom);
        await db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = classroom.Id }, classroom);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, ClassroomRequest request, CancellationToken cancellationToken)
    {
        if (Invalid(request)) return BadRequest("Номер аудитории и список оборудования обязательны.");
        var classroom = await db.Classrooms.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (classroom is null) return NotFound();
        classroom.Update(request.Number, request.Equipment);
        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var classroom = await db.Classrooms.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (classroom is null) return NotFound();
        if (await db.Reports.AnyAsync(item => item.ClassroomId == id, cancellationToken))
            return Conflict("Аудитория используется в отчёте.");
        db.Classrooms.Remove(classroom);
        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private static bool Invalid(ClassroomRequest request) =>
        string.IsNullOrWhiteSpace(request.Number) || request.Equipment is null ||
        request.Equipment.Any(string.IsNullOrWhiteSpace);
}
