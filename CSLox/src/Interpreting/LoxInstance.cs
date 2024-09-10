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

        LoxFunction? method = myClass.FindMethod(name.lexeme);
        if (method != null) return method;

        throw new Error.RuntimeError(name, $"Can not get property '{name.lexeme}'. It is not defined in {myClass}, as either a field or method. Is this a typo perhaps?");
    }
    
    public void Set(Token name, object value)
    {
        fields[name.lexeme] = value;
    }
    
    public override string ToString()
    {
        return $"instance of {myClass}";
    }
}