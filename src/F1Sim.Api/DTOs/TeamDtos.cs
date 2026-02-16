namespace F1Sim.Api.DTOs;

public class TeamDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Constructor { get; set; }
    public int FoundedYear { get; set; }
    public required string BaseLocation { get; set; }
    public int ChampionshipsWon { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateTeamDto
{
    public required string Name { get; set; }
    public required string Constructor { get; set; }
    public int FoundedYear { get; set; }
    public required string BaseLocation { get; set; }
    public int ChampionshipsWon { get; set; }
}

public class UpdateTeamDto
{
    public required string Name { get; set; }
    public required string Constructor { get; set; }
    public int FoundedYear { get; set; }
    public required string BaseLocation { get; set; }
    public int ChampionshipsWon { get; set; }
}
