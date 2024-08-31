using CSLox.Scanning;

namespace CSLox.Parsing;

internal class Interpreter : Expression.IVisitor<object>
{
    public void Interpret(Expression expression)
    {
        try
        {
            object value = Evaluate(expression);
            Console.WriteLine(Stringify(value));
        }
        catch (Error.BaseException error)
        {
            Error.Report(error);
        }
    }

    private string Stringify(object o)
    {
        if (o == null) return "nil";

        if (o is double) {
            string text = o.ToString()!;
            if (text.EndsWith(".0"))
            {
                text = Program.Substring(text, 0, text.Length - 2);
            }
            return text;
        }

        return o.ToString()!;
    }

    public object VisitBinaryExpression(Expression.Binary expression)
    {
        object left = Evaluate(expression.left);
        object right = Evaluate(expression.right);

        switch (expression.operatorToken.type)
        {
            case TokenType.GREATER:
                CheckNumberOperands(expression.operatorToken, left, right);
                return (double)left > (double)right;
            case TokenType.GREATER_EQUAL:
                CheckNumberOperands(expression.operatorToken, left, right);
                return (double)left >= (double)right;
            case TokenType.LESS:
                CheckNumberOperands(expression.operatorToken, left, right);
                return (double)left < (double)right;
            case TokenType.LESS_EQUAL:
                CheckNumberOperands(expression.operatorToken, left, right);
                return (double)left <= (double)right;
            case TokenType.MINUS:
                CheckNumberOperands(expression.operatorToken, left, right);
                return (double)left - (double)right;
            case TokenType.SLASH:
                CheckNumberOperands(expression.operatorToken, left, right);
                return (double)left / (double)right;
            case TokenType.STAR:
                CheckNumberOperands(expression.operatorToken, left, right);
                return (double)left * (double)right;
            case TokenType.PLUS:
                if (left is double && right is double) return (double)left + (double)right;
                if (left is string && right is string) return (string)left + (string)right;
                throw new Error.RuntimeError(expression.operatorToken, "Operands must be two numbers or two strings.");
            case TokenType.BANG_EQUAL: return !IsEqual(left, right);
            case TokenType.EQUAL_EQUAL: return IsEqual(left, right);
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
            case TokenType.MINUS:
                CheckNumberOperand(expression.operatorToken, right);
                return -(double)right;
            case TokenType.BANG:
                return !IsTruthy(right);
        }

        // Unreachable.
        throw new Error.UnreachableCodeWasReachedError();
    }

    //| HELPER
    private object Evaluate(Expression expression) => expression.Accept(this);

    //| HELPER
    private void CheckNumberOperands(Token operatorToken, object left, object right)
    {
        if (left is double && right is double) return;
        throw new Error.RuntimeError(operatorToken, "Operands must be numbers.");
    }

    //| HELPER
    private void CheckNumberOperand(Token operatorToken, object operand)
    {
        if (operand is double) return;
        throw new Error.RuntimeError(operatorToken, "Operand must be a number.");
    }

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