using System.ComponentModel.DataAnnotations;

namespace dotnet6_webapi.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [MaxLength(10)]
    public string Account { get; set; }
    [MaxLength(10)]
    public string Password { get; set; }
    [MaxLength(10)]
    public string? FirstName { get; set; }

    [MaxLength(10)]
    public string? MiddleName { get; set; }

    [MaxLength(10)]
    public string? LastName { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(100)]
    public string? Phone { get; set; }

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    // 可選的導覽屬性（不影響侵入性）
    public ICollection<UserRefreshToken>? UserRefreshTokens { get; set; }
}