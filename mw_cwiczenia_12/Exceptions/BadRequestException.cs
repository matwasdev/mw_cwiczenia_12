namespace mw_cwiczenia_12.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException() {}
    public BadRequestException(string message) : base(message) {}
    
}