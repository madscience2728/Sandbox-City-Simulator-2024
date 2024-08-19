namespace Sandbox_Simulator_2024.Scripting.Parsing;

public class ParseResult
{
    public bool Success { get; }
    public string Message { get; }

    public ParseResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}
