using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavelevCrossPlatform.Dtos;
using SavelevCrossPlatform.Services;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Route("api/lab-works")]
public class LabWorksController : ControllerBase
{
    private readonly LabWorkService _works;

    public LabWorksController(LabWorkService works)
    {
        _works = works;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LabWorkDto>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await _works.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LabWorkDto>> Get(int id, CancellationToken cancellationToken)
    {
        var work = await _works.GetAsync(id, cancellationToken);
        return work is null ? NotFound() : Ok(work);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<LabWorkDto>> Create(LabWorkDto request, CancellationToken cancellationToken)
    {
        if (Invalid(request)) return BadRequest("Название, указания, дисциплина и оборудование обязательны.");
        var work = await _works.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = work.Id }, work);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, LabWorkDto request, CancellationToken cancellationToken)
    {
        if (Invalid(request)) return BadRequest("Название, указания, дисциплина и оборудование обязательны.");
        return await _works.UpdateAsync(id, request, cancellationToken) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await _works.DeleteAsync(id, cancellationToken) switch
        {
            DeleteResult.NotFound => NotFound(),
            DeleteResult.InUse => Conflict("Работа используется в отчёте."),
            _ => NoContent()
        };
    }

    private static bool Invalid(LabWorkDto request) =>
        string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Instructions) ||
        string.IsNullOrWhiteSpace(request.Discipline) || string.IsNullOrWhiteSpace(request.RequiredEquipment);
}
