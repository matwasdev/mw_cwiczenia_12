namespace mw_cwiczenia_12.Exceptions;

public class ConflictException : Exception
{
    public ConflictException() {}
    public ConflictException(string message) : base(message) {}
    
}