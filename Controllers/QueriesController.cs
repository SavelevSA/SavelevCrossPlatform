using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavelevCrossPlatform.Services;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Authorize]
[Route("api/queries")]
public class QueriesController : ControllerBase
{
    private readonly QueryService _queries;

    public QueriesController(QueryService queries)
    {
        _queries = queries;
    }

    [HttpGet("report-details")]
    public async Task<IActionResult> ReportDetails([FromQuery] bool? passed, CancellationToken cancellationToken)
    {
        return Ok(await _queries.ReportDetailsAsync(passed, cancellationToken));
    }

    [HttpGet("teachers/{teacherId:int}/works")]
    public async Task<IActionResult> TeacherWorks(int teacherId, CancellationToken cancellationToken)
    {
        var result = await _queries.TeacherWorksAsync(teacherId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("classrooms/{classroomId:int}/works")]
    public async Task<IActionResult> ClassroomWorks(int classroomId, CancellationToken cancellationToken)
    {
        var result = await _queries.ClassroomWorksAsync(classroomId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
