using apbd11.DTOs;
using apbd11.Models;
using apbd11.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd11.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionController : ControllerBase
{
    private readonly IDbService _dbService;

    public PrescriptionController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("/patient/{id}")]
    public async Task<ActionResult> GetPrescription(int id)
    {
        var prescriptions = await _dbService.GetPrescription(id);
        return Ok(prescriptions);
    }

    [HttpPost]
    public async Task<ActionResult> AddPrescription(PrescriptionRequestDto prescription)
    {
        try
        {
            await _dbService.AddNewPrescription(prescription);
            return Ok("prescription added");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
}