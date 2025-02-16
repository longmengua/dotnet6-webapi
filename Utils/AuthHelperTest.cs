using System.Security.Claims;
using Xunit;

namespace dotnet6_webapi.Utils;
public class AuthHelperTests
{
    private readonly string _validSecretKey = "defaultSuperSecretKeyForJwt";
    private readonly string _validIssuer = "JWTIssuer";
    private readonly string _validAudience = "JWTAudience";

    public AuthHelperTests()
    {
        AuthHelper.SetConfiguration(_validSecretKey, _validIssuer, _validAudience);
    }

    [Fact]
    public void GenerateToken_ShouldReturn_ValidToken()
    {
        // Arrange
        string account = "testUser";

        // Act
        string token = AuthHelper.GenerateToken(account, 1);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public void VerifyToken_ShouldReturn_True_WithValidToken()
    {
        // Arrange
        string account = "testUser";
        string token = AuthHelper.GenerateToken(account, 1);

        // Act
        bool isValid = AuthHelper.VerifyToken(token, out ClaimsPrincipal? principal);

        // Assert
        Assert.True(isValid);
        Assert.NotNull(principal);
        Assert.Equal(account, principal?.Identity?.Name);
    }

    [Fact]
    public void VerifyToken_ShouldReturn_False_WithInvalidToken()
    {
        // Arrange
        string invalidToken = "invalid.jwt.token";

        // Act
        bool isValid = AuthHelper.VerifyToken(invalidToken, out ClaimsPrincipal? principal);

        // Assert
        Assert.False(isValid);
        Assert.Null(principal);
    }

    [Fact]
    public void VerifyToken_ShouldReturn_False_WithExpiredToken()
    {
        // Arrange
        string account = "expiredUser";
        string expiredToken = AuthHelper.GenerateToken(account, expiryHours: -1); // 過期 Token

        // Act
        bool isValid = AuthHelper.VerifyToken(expiredToken, out ClaimsPrincipal? principal);

        // Assert
        Assert.False(isValid);
        Assert.Null(principal);
    }
}
