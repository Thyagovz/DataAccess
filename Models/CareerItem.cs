namespace DataAccess.Models;

public class CareerItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public required Course Course { get; set; } 
}