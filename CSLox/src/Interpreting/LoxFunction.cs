
namespace CSLox;

internal class LoxFunction : ICallLoxFunctions
{
    public Statement.FunctionStatement declaration;
    public LoxEnvironment closure;
    
    public LoxFunction(Statement.FunctionStatement declaration, LoxEnvironment closure)
    {
        this.declaration = declaration;
        this.closure = closure;
    }

    public int Arity()
    {
        return declaration.parameters.Count;
    }

    public object? Call(Interpreter interpreter, List<object> arguments)
    {
        LoxEnvironment environment = new LoxEnvironment(closure);
        for (int i = 0; i < declaration.parameters.Count; i++)
        {
            environment.Define(declaration.parameters[i].lexeme, arguments[i]);
        }
        
        try
        {
            interpreter.ExecuteBlock(declaration.body, environment);
        }
        catch (Return returnValue)
        {
            return returnValue.value;
        }
        return null!;
    }
}
