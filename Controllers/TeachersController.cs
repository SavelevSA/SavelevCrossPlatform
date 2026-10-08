using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Data;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Route("api/teachers")]
public class TeachersController(LabDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Teacher>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await db.Teachers.AsNoTracking().ToArrayAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Teacher>> Get(int id, CancellationToken cancellationToken)
    {
        var teacher = await db.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        return teacher is null ? NotFound() : Ok(teacher);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Teacher>> Create(TeacherRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Discipline))
            return BadRequest("Имя и дисциплина обязательны.");

        var teacher = new Teacher(0, request.Name, request.Discipline);
        db.Teachers.Add(teacher);
        await db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = teacher.Id }, teacher);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, TeacherRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Discipline))
            return BadRequest("Имя и дисциплина обязательны.");

        var teacher = await db.Teachers.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (teacher is null) return NotFound();
        teacher.Update(request.Name, request.Discipline);
        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var teacher = await db.Teachers.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (teacher is null) return NotFound();
        if (await db.Reports.AnyAsync(item => item.TeacherId == id, cancellationToken))
            return Conflict("Преподаватель используется в отчёте.");
        db.Teachers.Remove(teacher);
        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
