using System.ComponentModel.DataAnnotations;

namespace Kol2.Entities;

public class ClientCategory
{
    [Key]
    public int IdClientCategory { get; set; }
    public string? Name { get; set; }
    public int Discount { get; set; }
}