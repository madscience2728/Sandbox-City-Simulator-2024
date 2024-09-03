
namespace CSLox;

internal class LoxFunction : ICallLoxFunctions
{
    public Statement.FunctionStatement declaration;
    
    public LoxFunction(Statement.FunctionStatement declaration)
    {
        this.declaration = declaration;
    }

    public int Arity()
    {
        return declaration.parameters.Count;
    }

    public object Call(Interpreter interpreter, List<object> arguments)
    {
        LoxEnvironment environment = new LoxEnvironment(interpreter.globals);
        for (int i = 0; i < declaration.parameters.Count; i++)
        {
            environment.Define(declaration.parameters[i].lexeme, arguments[i]);
        }
        
        interpreter.ExecuteBlock(declaration.body, environment);
        return null!;
    }
}
