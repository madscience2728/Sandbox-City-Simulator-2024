
namespace CSLox;

internal class LoxFunction : ICallLoxFunctions
{
    public Statement.FunctionStatement declaration;
    public LoxEnvironment closure;
    
    bool isInitializer = false;
    
    public LoxFunction(Statement.FunctionStatement declaration, LoxEnvironment closure, bool isInitializer)
    {
        this.declaration = declaration;
        this.closure = closure;
        this.isInitializer = isInitializer;
    }
    
    public LoxFunction Bind(LoxInstance instance)
    {
        LoxEnvironment environment = new LoxEnvironment(closure);
        environment.Define("this", instance);
        return new LoxFunction(declaration, environment, isInitializer);
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
            if (isInitializer) return closure.GetAt(0, "this");
            return returnValue.value;
        }

        if (isInitializer) return closure.GetAt(0, "this");
        return null!;
    }
}
