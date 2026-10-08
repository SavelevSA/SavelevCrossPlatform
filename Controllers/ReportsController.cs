using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Data;
using SavelevCrossPlatform.Models;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Authorize]
[Route("api/reports")]
public class ReportsController(LabDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Report>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await db.Reports.AsNoTracking().ToArrayAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Report>> Get(int id, CancellationToken cancellationToken)
    {
        var report = await db.Reports.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        return report is null ? NotFound() : Ok(report);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Report>> Create(ReportRequest request, CancellationToken cancellationToken)
    {
        var (teacher, work, classroom) = await ResolveAsync(request, cancellationToken);
        if (teacher is null || work is null || classroom is null)
            return BadRequest("Указаны несуществующие преподаватель, работа или аудитория.");
        var report = new Report(0, teacher, work, classroom, request.DeviationPercent);
        db.Reports.Add(report);
        await db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = report.Id }, report);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, ReportRequest request, CancellationToken cancellationToken)
    {
        var report = await db.Reports.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (report is null) return NotFound();
        var (teacher, work, classroom) = await ResolveAsync(request, cancellationToken);
        if (teacher is null || work is null || classroom is null)
            return BadRequest("Указаны несуществующие преподаватель, работа или аудитория.");
        report.Recheck(teacher, work, classroom, request.DeviationPercent);
        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var report = await db.Reports.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (report is null) return NotFound();
        db.Reports.Remove(report);
        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private async Task<(Teacher?, LabWork?, Classroom?)> ResolveAsync(
        ReportRequest request, CancellationToken cancellationToken)
    {
        var teacher = await db.Teachers.FirstOrDefaultAsync(
            item => item.Id == request.TeacherId, cancellationToken);
        var work = await db.Works.FirstOrDefaultAsync(
            item => item.Id == request.LabWorkId, cancellationToken);
        var classroom = await db.Classrooms.FirstOrDefaultAsync(
            item => item.Id == request.ClassroomId, cancellationToken);
        return (teacher, work, classroom);
    }
}
