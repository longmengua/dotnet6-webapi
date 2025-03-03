namespace dotnet6_webapi.Models;

/// <summary>
/// 建立 Refresh token 於資料庫，token 為 16 位元
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// 唯一識別碼
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 16 位元 Refresh Token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Token 建立時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTimeOffset.UtcNow.UtcDateTime;

    /// <summary>
    /// 是否已使用過
    /// </summary>
    public bool IsUsed { get; set; } = false;

    /// <summary>
    /// 是否已撤銷
    /// </summary>
    public bool IsRevoked { get; set; } = false;

    /// <summary>
    /// 確認 Token 是否仍有效（未使用、未撤銷）
    /// </summary>
    public bool IsValid => !IsUsed && !IsRevoked;

    // 可選的導覽屬性
    public ICollection<UserRefreshToken>? UserRefreshTokens { get; set; }
}
