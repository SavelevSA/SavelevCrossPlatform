using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavelevCrossPlatform.Dtos;
using SavelevCrossPlatform.Services;

namespace SavelevCrossPlatform.Controllers;

[ApiController]
[Route("api/teachers")]
public class TeachersController : ControllerBase
{
    private readonly TeacherService _teachers;

    public TeachersController(TeacherService teachers)
    {
        _teachers = teachers;
    }

    [HttpGet]
    public async Task<ActionResult<List<TeacherDto>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await _teachers.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TeacherDto>> Get(int id, CancellationToken cancellationToken)
    {
        var teacher = await _teachers.GetAsync(id, cancellationToken);
        return teacher is null ? NotFound() : Ok(teacher);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TeacherDto>> Create(TeacherDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Discipline))
            return BadRequest("Имя и дисциплина обязательны.");

        var teacher = await _teachers.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = teacher.Id }, teacher);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, TeacherDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Discipline))
            return BadRequest("Имя и дисциплина обязательны.");

        return await _teachers.UpdateAsync(id, request, cancellationToken) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await _teachers.DeleteAsync(id, cancellationToken) switch
        {
            DeleteResult.NotFound => NotFound(),
            DeleteResult.InUse => Conflict("Преподаватель используется в отчёте."),
            _ => NoContent()
        };
    }
}
