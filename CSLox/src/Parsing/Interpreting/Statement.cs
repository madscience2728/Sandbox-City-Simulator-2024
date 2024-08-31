namespace CSLox;

using CSLox.Parsing;

internal class Statement
{
    public Statement Print(Expression expression)
    {
        Console.WriteLine(expression);
        throw new System.NotImplementedException();
    }
    
    public Statement Expression(Expression expression)
    {
        throw new System.NotImplementedException();
    }
}