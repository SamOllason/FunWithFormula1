namespace F1Sim.Api.DTOs;

public class DriverDto
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Nationality { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int DriverNumber { get; set; }
    public int? TeamId { get; set; }
    public string? TeamName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateDriverDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Nationality { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int DriverNumber { get; set; }
    public int? TeamId { get; set; }
}

public class UpdateDriverDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Nationality { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int DriverNumber { get; set; }
    public int? TeamId { get; set; }
}
