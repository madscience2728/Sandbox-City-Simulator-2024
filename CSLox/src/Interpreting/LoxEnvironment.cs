
namespace CSLox;

internal class LoxEnvironment
{
    private Dictionary<string, object?> values = new Dictionary<string, object?>();
    private LoxEnvironment? enclosing;
    
    public LoxEnvironment()
    {
        enclosing = null;
    }
    
    public LoxEnvironment(LoxEnvironment enclosing)
    {
        this.enclosing = enclosing;
    }
    
    public void Define(string name, object? value)
    {
        values[name] = value;
    }

    public object? Get(Token name)
    {
        if (values.ContainsKey(name.lexeme))
        {
            return values[name.lexeme];
        }

        if (enclosing != null) return enclosing.Get(name);

        throw new Error.RuntimeError(name, "Undefined variable '" + name.lexeme + "'.");
    }

    public void Assign(Token name, object value)
    {
        if (values.ContainsKey(name.lexeme))
        {
            values[name.lexeme] = value;
            return;
        }

        if (enclosing != null)
        {
            enclosing.Assign(name, value);
            return;
        }

        throw new Error.RuntimeError(name, "Undefined variable '" + name.lexeme + "'.");
    }
}