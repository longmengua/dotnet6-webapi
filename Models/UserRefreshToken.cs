namespace dotnet6_webapi.Models;

/// <summary>
/// 用於建立 User 和 RefreshToken 之間的多對多關聯
/// </summary>
public class UserRefreshToken
{
    public int UserId { get; set; }
    public int RefreshTokenId { get; set; }

    // 導覽屬性 (Navigation Property) - 可選
    public User? User { get; set; }
    public RefreshToken? RefreshToken { get; set; }
}
