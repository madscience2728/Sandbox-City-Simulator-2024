


namespace CSLox;

// A dictionary with scope.
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

        throw new Error.RuntimeError(name, "Tried to get an undefined variable '" + name.lexeme + "'.");
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

        throw new Error.RuntimeError(name, "Tried to assign an undefined variable '" + name.lexeme + "'.");
    }

    public object GetAt(int distance, string lexeme)
    {
        return Ancestor(distance).values[lexeme]!;
    }

    LoxEnvironment Ancestor(int distance)
    {
        LoxEnvironment environment = this;
        for (int i = 0; i < distance; i++)
        {
            environment = environment.enclosing!;
        }

        return environment;
    }

    internal void AssignAt(int distance, Token name, object value)
    {
        Ancestor(distance).values[name.lexeme] = value;
    }
}