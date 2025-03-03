namespace dotnet6_webapi.DTO.Req;

public class UpdatePassword
{
    public string? Account { get; set; }
    public string? NewPassword { get; set; }
}
