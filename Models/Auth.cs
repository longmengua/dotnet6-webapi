using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet6_webapi.Models;

public class Auth
{
    [Key]
    public int Id { get; set; }

    [MaxLength(32)]
    public string Account { get; set; }

    [MaxLength(32)]
    public string Password { get; set; }

    [MaxLength(16)]
    public string Token { get; set; }

    [MaxLength(20)]
    public string WebIp { get; set; }

    [MaxLength(20)]
    public string TableauIp { get; set; }

    [MaxLength(20)]
    public string MobileIp { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public virtual User User { get; set; }

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}