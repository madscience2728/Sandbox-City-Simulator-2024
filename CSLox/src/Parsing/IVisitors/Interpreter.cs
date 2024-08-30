namespace CSLox.Parsing;

internal class Interpreter : Expression.IVisitor<object>
{
    public object VisitBinaryExpression(Expression.Binary expression)
    {
        object left = Evaluate(expression.left);
        object right = Evaluate(expression.right);

        switch (expression.operatorToken.type)
        {
            case Scanning.TokenType.GREATER:
                return (double)left > (double)right;
            case Scanning.TokenType.GREATER_EQUAL:
                return (double)left >= (double)right;
            case Scanning.TokenType.LESS:
                return (double)left < (double)right;
            case Scanning.TokenType.LESS_EQUAL:
                return (double)left <= (double)right;
            case Scanning.TokenType.MINUS:
                return (double)left - (double)right;
            case Scanning.TokenType.SLASH:
                return (double)left / (double)right;
            case Scanning.TokenType.STAR:
                return (double)left * (double)right;
            case Scanning.TokenType.PLUS:
                if (left is double && right is double) return (double)left + (double)right;
                if (left is string && right is string) return (string)left + (string)right;
                break;
            case Scanning.TokenType.BANG_EQUAL: return !IsEqual(left, right);
            case Scanning.TokenType.EQUAL_EQUAL: return IsEqual(left, right);
        }

        // Unreachable.
        throw new Error.UnreachableCodeWasReachedError();
    }

    public object VisitGroupingExpression(Expression.Grouping expression)
    {
        return Evaluate(expression.expression);
    }

    public object VisitLiteralExpression(Expression.Literal expression)
    {
        return expression.value;
    }

    public object VisitUnaryExpression(Expression.Unary expression)
    {
        object right = Evaluate(expression.right);

        switch (expression.operatorToken.type)
        {
            case Scanning.TokenType.MINUS:
                return -(double)right;
            case Scanning.TokenType.BANG:
                return !IsTruthy(right);
        }

        // Unreachable.
        throw new Error.UnreachableCodeWasReachedError();
    }

    //| HELPER
    private object Evaluate(Expression expression) => expression.Accept(this);

    //| HELPER
    private bool IsEqual(object left, object right)
    {
        if (left == null && right == null) return true;
        if (left == null) return false;
        return left.Equals(right);
    }    

    //| HELPER
    private bool IsTruthy(object right)
    {
        if (right == null) return false;
        if (right is bool boolean) return boolean;
        if (right is double number) return number != 0.0;
        return true;
    }
}