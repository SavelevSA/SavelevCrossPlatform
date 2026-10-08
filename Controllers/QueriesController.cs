using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavelevCrossPlatform.Data;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Authorize]
[Route("api/queries")]
public class QueriesController(LabDbContext db) : ControllerBase
{
    [HttpGet("report-details")]
    public async Task<IActionResult> ReportDetails([FromQuery] bool? passed, CancellationToken cancellationToken)
    {
        var result = await (
            from report in db.Reports.AsNoTracking()
            join teacher in db.Teachers.AsNoTracking() on report.TeacherId equals teacher.Id
            join work in db.Works.AsNoTracking() on report.LabWorkId equals work.Id
            join classroom in db.Classrooms.AsNoTracking() on report.ClassroomId equals classroom.Id
            where !passed.HasValue || report.Passed == passed.Value
            select new
            {
                report.Id,
                Teacher = teacher.Name,
                Work = work.Title,
                Classroom = classroom.Number,
                report.DeviationPercent,
                report.Passed,
                report.Conclusion,
                report.CheckedAt
            }).ToArrayAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("teachers/{teacherId:int}/works")]
    public async Task<IActionResult> TeacherWorks(int teacherId, CancellationToken cancellationToken)
    {
        var teacher = await db.Teachers.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == teacherId, cancellationToken);
        if (teacher is null) return NotFound();
        var works = await db.Works.AsNoTracking().ToArrayAsync(cancellationToken);
        var result = works
            .Where(teacher.CanSupervise)
            .Select(work => new { Teacher = teacher.Name, Work = work.Title, work.Id })
            .ToArray();
        return Ok(result);
    }

    [HttpGet("classrooms/{classroomId:int}/works")]
    public async Task<IActionResult> ClassroomWorks(int classroomId, CancellationToken cancellationToken)
    {
        var classroom = await db.Classrooms.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == classroomId, cancellationToken);
        if (classroom is null) return NotFound();
        var works = await db.Works.AsNoTracking().ToArrayAsync(cancellationToken);
        var result = works
            .Where(classroom.CanHost)
            .Select(work => new { Classroom = classroom.Number, Work = work.Title, work.Id })
            .ToArray();
        return Ok(result);
    }
}
