using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavelevCrossPlatform.Dtos;
using SavelevCrossPlatform.Services;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Authorize]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly ReportService _reports;

    public ReportsController(ReportService reports)
    {
        _reports = reports;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReportDto>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await _reports.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReportDto>> Get(int id, CancellationToken cancellationToken)
    {
        var report = await _reports.GetAsync(id, cancellationToken);
        return report is null ? NotFound() : Ok(report);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ReportDto>> Create(ReportDto request, CancellationToken cancellationToken)
    {
        var report = await _reports.CreateAsync(request, cancellationToken);
        if (report is null)
            return BadRequest("Указаны несуществующие преподаватель, работа или аудитория.");
        return CreatedAtAction(nameof(Get), new { id = report.Id }, report);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, ReportDto request, CancellationToken cancellationToken)
    {
        return await _reports.UpdateAsync(id, request, cancellationToken) switch
        {
            ReportUpdateResult.NotFound => NotFound(),
            ReportUpdateResult.InvalidReferences =>
                BadRequest("Указаны несуществующие преподаватель, работа или аудитория."),
            _ => NoContent()
        };
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await _reports.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
    }
}
