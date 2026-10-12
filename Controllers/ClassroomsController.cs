using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavelevCrossPlatform.Dtos;
using SavelevCrossPlatform.Services;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Route("api/classrooms")]
public class ClassroomsController : ControllerBase
{
    private readonly ClassroomService _classrooms;

    public ClassroomsController(ClassroomService classrooms)
    {
        _classrooms = classrooms;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassroomDto>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await _classrooms.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClassroomDto>> Get(int id, CancellationToken cancellationToken)
    {
        var classroom = await _classrooms.GetAsync(id, cancellationToken);
        return classroom is null ? NotFound() : Ok(classroom);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ClassroomDto>> Create(ClassroomDto request, CancellationToken cancellationToken)
    {
        if (Invalid(request)) return BadRequest("Номер аудитории и список оборудования обязательны.");
        var classroom = await _classrooms.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = classroom.Id }, classroom);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, ClassroomDto request, CancellationToken cancellationToken)
    {
        if (Invalid(request)) return BadRequest("Номер аудитории и список оборудования обязательны.");
        return await _classrooms.UpdateAsync(id, request, cancellationToken) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await _classrooms.DeleteAsync(id, cancellationToken) switch
        {
            DeleteResult.NotFound => NotFound(),
            DeleteResult.InUse => Conflict("Аудитория используется в отчёте."),
            _ => NoContent()
        };
    }

    private static bool Invalid(ClassroomDto request) =>
        string.IsNullOrWhiteSpace(request.Number) || request.Equipment is null ||
        request.Equipment.Any(string.IsNullOrWhiteSpace);
}
