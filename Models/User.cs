using System.ComponentModel.DataAnnotations;

namespace dotnet6_webapi.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [MaxLength(10)]
    public string FirstName { get; set; }

    [MaxLength(10)]
    public string MiddleName { get; set; }

    [MaxLength(10)]
    public string LastName { get; set; }

    [MaxLength(100)]
    public string Email { get; set; }

    [MaxLength(100)]
    public string Phone { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}