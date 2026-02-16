namespace F1Sim.Domain.Entities;

public class Driver : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Nationality { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int DriverNumber { get; set; }
    
    public int? TeamId { get; set; }
    public Team? Team { get; set; }
}
