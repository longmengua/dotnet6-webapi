namespace dotnet6_webapi.Exceptions;

public class CustomException : Exception
{
    public CustomException(string message) : base(message)
    {
    }
}