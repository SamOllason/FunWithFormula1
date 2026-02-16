using F1Sim.Api.DTOs;
using F1Sim.Domain.Entities;
using F1Sim.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace F1Sim.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly F1SimDbContext _context;
    private readonly ILogger<DriversController> _logger;

    public DriversController(F1SimDbContext context, ILogger<DriversController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DriverDto>>> GetDrivers()
    {
        try
        {
            var drivers = await _context.Drivers
                .Include(d => d.Team)
                .Select(d => new DriverDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Nationality = d.Nationality,
                    DateOfBirth = d.DateOfBirth,
                    DriverNumber = d.DriverNumber,
                    TeamId = d.TeamId,
                    TeamName = d.Team != null ? d.Team.Name : null,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                })
                .ToListAsync();

            return Ok(drivers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving drivers");
            return StatusCode(500, "An error occurred while retrieving drivers");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DriverDto>> GetDriver(int id)
    {
        try
        {
            var driver = await _context.Drivers
                .Include(d => d.Team)
                .Where(d => d.Id == id)
                .Select(d => new DriverDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Nationality = d.Nationality,
                    DateOfBirth = d.DateOfBirth,
                    DriverNumber = d.DriverNumber,
                    TeamId = d.TeamId,
                    TeamName = d.Team != null ? d.Team.Name : null,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (driver == null)
            {
                return NotFound($"Driver with ID {id} not found");
            }

            return Ok(driver);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving driver {DriverId}", id);
            return StatusCode(500, "An error occurred while retrieving the driver");
        }
    }

    [HttpPost]
    public async Task<ActionResult<DriverDto>> CreateDriver(CreateDriverDto createDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(createDto.FirstName) || createDto.FirstName.Length > 50)
            {
                return BadRequest("FirstName is required and must be 50 characters or less");
            }

            if (string.IsNullOrWhiteSpace(createDto.LastName) || createDto.LastName.Length > 50)
            {
                return BadRequest("LastName is required and must be 50 characters or less");
            }

            if (string.IsNullOrWhiteSpace(createDto.Nationality) || createDto.Nationality.Length > 50)
            {
                return BadRequest("Nationality is required and must be 50 characters or less");
            }

            if (createDto.DriverNumber < 1 || createDto.DriverNumber > 99)
            {
                return BadRequest("DriverNumber must be between 1 and 99");
            }

            var existingDriver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.DriverNumber == createDto.DriverNumber);
            
            if (existingDriver != null)
            {
                return BadRequest($"Driver number {createDto.DriverNumber} is already taken");
            }

            if (createDto.TeamId.HasValue)
            {
                var teamExists = await _context.Teams.AnyAsync(t => t.Id == createDto.TeamId.Value);
                if (!teamExists)
                {
                    return BadRequest($"Team with ID {createDto.TeamId} does not exist");
                }
            }

            var driver = new Driver
            {
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Nationality = createDto.Nationality,
                DateOfBirth = createDto.DateOfBirth,
                DriverNumber = createDto.DriverNumber,
                TeamId = createDto.TeamId
            };

            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            var createdDriver = await _context.Drivers
                .Include(d => d.Team)
                .Where(d => d.Id == driver.Id)
                .Select(d => new DriverDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Nationality = d.Nationality,
                    DateOfBirth = d.DateOfBirth,
                    DriverNumber = d.DriverNumber,
                    TeamId = d.TeamId,
                    TeamName = d.Team != null ? d.Team.Name : null,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                })
                .FirstAsync();

            return CreatedAtAction(nameof(GetDriver), new { id = createdDriver.Id }, createdDriver);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating driver");
            return StatusCode(500, "An error occurred while creating the driver");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DriverDto>> UpdateDriver(int id, UpdateDriverDto updateDto)
    {
        try
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound($"Driver with ID {id} not found");
            }

            if (string.IsNullOrWhiteSpace(updateDto.FirstName) || updateDto.FirstName.Length > 50)
            {
                return BadRequest("FirstName is required and must be 50 characters or less");
            }

            if (string.IsNullOrWhiteSpace(updateDto.LastName) || updateDto.LastName.Length > 50)
            {
                return BadRequest("LastName is required and must be 50 characters or less");
            }

            if (string.IsNullOrWhiteSpace(updateDto.Nationality) || updateDto.Nationality.Length > 50)
            {
                return BadRequest("Nationality is required and must be 50 characters or less");
            }

            if (updateDto.DriverNumber < 1 || updateDto.DriverNumber > 99)
            {
                return BadRequest("DriverNumber must be between 1 and 99");
            }

            var existingDriver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.DriverNumber == updateDto.DriverNumber && d.Id != id);
            
            if (existingDriver != null)
            {
                return BadRequest($"Driver number {updateDto.DriverNumber} is already taken");
            }

            if (updateDto.TeamId.HasValue)
            {
                var teamExists = await _context.Teams.AnyAsync(t => t.Id == updateDto.TeamId.Value);
                if (!teamExists)
                {
                    return BadRequest($"Team with ID {updateDto.TeamId} does not exist");
                }
            }

            driver.FirstName = updateDto.FirstName;
            driver.LastName = updateDto.LastName;
            driver.Nationality = updateDto.Nationality;
            driver.DateOfBirth = updateDto.DateOfBirth;
            driver.DriverNumber = updateDto.DriverNumber;
            driver.TeamId = updateDto.TeamId;

            await _context.SaveChangesAsync();

            var updatedDriver = await _context.Drivers
                .Include(d => d.Team)
                .Where(d => d.Id == driver.Id)
                .Select(d => new DriverDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Nationality = d.Nationality,
                    DateOfBirth = d.DateOfBirth,
                    DriverNumber = d.DriverNumber,
                    TeamId = d.TeamId,
                    TeamName = d.Team != null ? d.Team.Name : null,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                })
                .FirstAsync();

            return Ok(updatedDriver);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating driver {DriverId}", id);
            return StatusCode(500, "An error occurred while updating the driver");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDriver(int id)
    {
        try
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound($"Driver with ID {id} not found");
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting driver {DriverId}", id);
            return StatusCode(500, "An error occurred while deleting the driver");
        }
    }
}
