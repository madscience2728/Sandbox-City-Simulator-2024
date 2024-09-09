
namespace CSLox;

internal class LoxClass : ICallLoxFunctions
{
    string name;
    public Dictionary<string, LoxFunction> methods = new Dictionary<string, LoxFunction>();
    
    public LoxClass(string name)
    {
        this.name = name;
    }

    public int Arity()
    {
        return 0;
    }

    public object? Call(Interpreter interpreter, List<object> arguments)
    {
        LoxInstance instance = new LoxInstance(this);
        return instance;
    }

    public override string ToString()
    {
        return name;
    }
}