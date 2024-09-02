namespace CSLox;

internal class Interpreter : Expression.IVisitExpressions<object>, Statement.IVisitStatements<object>
{
    private LoxEnvironment environment = new LoxEnvironment();
    
    public void Interpret(List<Statement> statements)
    {
        try
        {
            foreach (Statement statement in statements)
            {
                Execute(statement);
            }
        }
        catch (Error.BaseException error)
        {
            Error.Report(error);
        }
    }
    
    //* I VISIT STATEMENTS

    //| IVisitStatements<object>
    public object VisitPrintStatement(Statement.PrintStatement statement)
    {
        object value = Evaluate(statement.expression);
        Console.Write(Program.Stringify(value));
        return null!;
    }

    //| IVisitStatements<object>
    public object VisitPrintLineStatement(Statement.PrintLineStatement statement)
    {
        object value = Evaluate(statement.expression);
        Console.WriteLine(Program.Stringify(value));
        return null!;
    }

    //| IVisitStatements<object>
    public object VisitExpressionStatement(Statement.ExpressionStatement statement)
    {
        Evaluate(statement.expression);
        return null!;
    } 
    
    //| IVisitStatements<object>
    public object VisitVariableDeclarationStatement(Statement.VariableDeclarationStatement statement)
    {
        object? value = null;
        if (statement.initializer != null)
        {
            value = Evaluate(statement.initializer);
        }
        
        environment.Define(statement.name.lexeme, value);
        return null!;
    }
    
    //* I VISIT EXPRESSIONS
    
    //| IVisitExpressions<object>
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

    //| IVisitExpressions<object>
    public object VisitGroupingExpression(Expression.Grouping expression)
    {
        return Evaluate(expression.expression);
    }

    //| IVisitExpressions<object>
    public object VisitLiteralExpression(Expression.Literal expression)
    {
        return expression.value;
    }

    //| IVisitExpressions<object>
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

    //| IVisitExpressions<object>
    public object VisitVariableExpression(Expression.Variable expression)
    {
        return environment.Get(expression.name)!;
    }

    //| IVisitExpressions<object>
    public object VisitAssignExpression(Expression.Assign expression)
    {
        object value = Evaluate(expression.value);
        environment.Assign(expression.name, value);
        return value;
    }
    
    //* HELPERS

    //| HELPERS
    private void Execute(Statement statement) => statement.Accept(this);
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
        return false;
    }
}