namespace mw_cwiczenia_12.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(){}
    public NotFoundException(string message) : base(message) {}
    
}