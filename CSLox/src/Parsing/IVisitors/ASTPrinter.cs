namespace CSLox.Parsing;

internal class AstPrinter : Expression.IVisitor<string>
{
    public string VisitBinaryExpression(Expression.Binary expression)
    {
        return Parenthesize(
            expression.operatorToken.lexeme, 
            expression.left, 
            expression.right
        );
    }

    public string VisitGroupingExpression(Expression.Grouping expression)
    {
        return Parenthesize("group", expression.expression);
    }

    public string VisitLiteralExpression(Expression.Literal expression)
    {
        if (expression.value == null) return "nil";
        return expression.value.ToString()!;
    }

    public string VisitUnaryExpression(Expression.Unary expression)
    {
        return Parenthesize(
            expression.operatorToken.lexeme, 
            expression.right
        );
    }

    public string Print(Expression expression) {
        return expression.Accept(this);
    }

    private string Parenthesize(string name, params Expression[] expressions)
    {
        System.Text.StringBuilder builder = new();

        builder.Append("(").Append(name);
        foreach (Expression expression in expressions)
        {
            builder.Append(" ");
            builder.Append(expression.Accept(this));
        }
        builder.Append(")");

        return builder.ToString();
    }
}