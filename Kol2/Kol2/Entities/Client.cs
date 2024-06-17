using System.ComponentModel.DataAnnotations;

namespace Kol2.Entities;

public class Client
{
    public int IdClient { get; set; }
    public int IdClientCategory { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    
}