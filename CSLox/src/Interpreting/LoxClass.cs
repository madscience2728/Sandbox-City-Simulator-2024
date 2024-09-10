
namespace CSLox;

internal class LoxClass : ICallLoxFunctions
{
    string name;
    public Dictionary<string, LoxFunction> methods = new();
    
    public LoxClass(string name, Dictionary<string, LoxFunction> methods)
    {
        this.name = name;
        this.methods = methods;
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
        return $"{name} class";
    }
    
    public LoxFunction? FindMethod(string name)
    {
        if (methods.ContainsKey(name))
        {
            return methods[name];
        }
        return null;
    }
}