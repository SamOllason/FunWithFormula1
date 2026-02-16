using F1Sim.Api.DTOs;
using F1Sim.Domain.Entities;
using F1Sim.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace F1Sim.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly F1SimDbContext _context;
    private readonly ILogger<TeamsController> _logger;

    public TeamsController(F1SimDbContext context, ILogger<TeamsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams()
    {
        try
        {
            var teams = await _context.Teams
                .Select(t => new TeamDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Constructor = t.Constructor,
                    FoundedYear = t.FoundedYear,
                    BaseLocation = t.BaseLocation,
                    ChampionshipsWon = t.ChampionshipsWon,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync();

            return Ok(teams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving teams");
            return StatusCode(500, "An error occurred while retrieving teams");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeamDto>> GetTeam(int id)
    {
        try
        {
            var team = await _context.Teams
                .Where(t => t.Id == id)
                .Select(t => new TeamDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Constructor = t.Constructor,
                    FoundedYear = t.FoundedYear,
                    BaseLocation = t.BaseLocation,
                    ChampionshipsWon = t.ChampionshipsWon,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (team == null)
            {
                return NotFound($"Team with ID {id} not found");
            }

            return Ok(team);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving team {TeamId}", id);
            return StatusCode(500, "An error occurred while retrieving the team");
        }
    }

    [HttpPost]
    public async Task<ActionResult<TeamDto>> CreateTeam(CreateTeamDto createDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(createDto.Name) || createDto.Name.Length > 100)
            {
                return BadRequest("Name is required and must be 100 characters or less");
            }

            if (string.IsNullOrWhiteSpace(createDto.Constructor) || createDto.Constructor.Length > 100)
            {
                return BadRequest("Constructor is required and must be 100 characters or less");
            }

            if (string.IsNullOrWhiteSpace(createDto.BaseLocation) || createDto.BaseLocation.Length > 100)
            {
                return BadRequest("BaseLocation is required and must be 100 characters or less");
            }

            if (createDto.FoundedYear < 1950 || createDto.FoundedYear > 2099)
            {
                return BadRequest("FoundedYear must be between 1950 and 2099");
            }

            var existingTeam = await _context.Teams
                .FirstOrDefaultAsync(t => t.Name == createDto.Name);
            
            if (existingTeam != null)
            {
                return BadRequest($"Team with name '{createDto.Name}' already exists");
            }

            var team = new Team
            {
                Name = createDto.Name,
                Constructor = createDto.Constructor,
                FoundedYear = createDto.FoundedYear,
                BaseLocation = createDto.BaseLocation,
                ChampionshipsWon = createDto.ChampionshipsWon
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            var createdTeam = await _context.Teams
                .Where(t => t.Id == team.Id)
                .Select(t => new TeamDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Constructor = t.Constructor,
                    FoundedYear = t.FoundedYear,
                    BaseLocation = t.BaseLocation,
                    ChampionshipsWon = t.ChampionshipsWon,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .FirstAsync();

            return CreatedAtAction(nameof(GetTeam), new { id = createdTeam.Id }, createdTeam);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating team");
            return StatusCode(500, "An error occurred while creating the team");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TeamDto>> UpdateTeam(int id, UpdateTeamDto updateDto)
    {
        try
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound($"Team with ID {id} not found");
            }

            if (string.IsNullOrWhiteSpace(updateDto.Name) || updateDto.Name.Length > 100)
            {
                return BadRequest("Name is required and must be 100 characters or less");
            }

            if (string.IsNullOrWhiteSpace(updateDto.Constructor) || updateDto.Constructor.Length > 100)
            {
                return BadRequest("Constructor is required and must be 100 characters or less");
            }

            if (string.IsNullOrWhiteSpace(updateDto.BaseLocation) || updateDto.BaseLocation.Length > 100)
            {
                return BadRequest("BaseLocation is required and must be 100 characters or less");
            }

            if (updateDto.FoundedYear < 1950 || updateDto.FoundedYear > 2099)
            {
                return BadRequest("FoundedYear must be between 1950 and 2099");
            }

            var existingTeam = await _context.Teams
                .FirstOrDefaultAsync(t => t.Name == updateDto.Name && t.Id != id);
            
            if (existingTeam != null)
            {
                return BadRequest($"Team with name '{updateDto.Name}' already exists");
            }

            team.Name = updateDto.Name;
            team.Constructor = updateDto.Constructor;
            team.FoundedYear = updateDto.FoundedYear;
            team.BaseLocation = updateDto.BaseLocation;
            team.ChampionshipsWon = updateDto.ChampionshipsWon;

            await _context.SaveChangesAsync();

            var updatedTeam = await _context.Teams
                .Where(t => t.Id == team.Id)
                .Select(t => new TeamDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Constructor = t.Constructor,
                    FoundedYear = t.FoundedYear,
                    BaseLocation = t.BaseLocation,
                    ChampionshipsWon = t.ChampionshipsWon,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .FirstAsync();

            return Ok(updatedTeam);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating team {TeamId}", id);
            return StatusCode(500, "An error occurred while updating the team");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeam(int id)
    {
        try
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound($"Team with ID {id} not found");
            }

            var hasDrivers = await _context.Drivers.AnyAsync(d => d.TeamId == id);
            if (hasDrivers)
            {
                return BadRequest("Cannot delete team with active drivers. Remove or reassign drivers first.");
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting team {TeamId}", id);
            return StatusCode(500, "An error occurred while deleting the team");
        }
    }
}
