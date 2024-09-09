namespace CSLox;

internal class LoxInstance
{
    LoxClass myClass;
    Dictionary<string, object> fields = new Dictionary<string, object>();

    public LoxInstance(LoxClass myClass)
    {
        this.myClass = myClass;
    }
    
    public object Get(Token name)
    {
        if (fields.ContainsKey(name.lexeme)) return fields[name.lexeme];
        throw new Error.RuntimeError(name, $"Can not get property '{name.lexeme}'. It is not defined in {myClass}.");
    }
    
    public void Set(Token name, object value)
    {
        fields[name.lexeme] = value;
    }
    
    public override string ToString()
    {
        return $"{myClass} instance";
    }
}