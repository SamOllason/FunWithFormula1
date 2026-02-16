namespace F1Sim.Domain.Entities;

public class Team : BaseEntity
{
    public required string Name { get; set; }
    public required string Constructor { get; set; }
    public int FoundedYear { get; set; }
    public required string BaseLocation { get; set; }
    public int ChampionshipsWon { get; set; }
    
    public ICollection<Driver> Drivers { get; set; } = new List<Driver>();
}
