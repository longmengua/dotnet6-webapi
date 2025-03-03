namespace dotnet6_webapi.DTO.Req
{
    public class Register
    {
        public string? Account { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; } = String.Empty;
        public string? MiddleName { get; set; } = String.Empty;
        public string? LastName { get; set; } = String.Empty;
        public string? Email { get; set; } = String.Empty;
        public string? Phone { get; set; } = String.Empty;
    }
}