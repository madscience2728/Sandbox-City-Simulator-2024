namespace CSLox;

internal class Interpreter : Expression.IVisitExpressions<object>, Statement.IVisitStatements<object>
{
    public LoxEnvironment environment { get; private set; }
    
    public LoxEnvironment globals { get; private set; }
    public Dictionary<Expression, int> locals = new Dictionary<Expression, int>();

    public Interpreter()
    {
        environment = globals = new LoxEnvironment();
        globals.Define("clock", new ClockFunction());
    }

    public void Interpret(List<Statement> statements)
    {
        try
        {
            foreach (Statement statement in statements)
            {
                Execute(statement);
            }
        }
        catch (Error.BaseError error)
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

    //| IVisitStatements<object>
    public object VisitBlockStatement(Statement.BlockStatement statement)
    {
        ExecuteBlock(statement.statements, new LoxEnvironment(environment));
        return null!;
    }

    //| IVisitStatements<object>
    public object VisitIfStatement(Statement.IfStatement statement)
    {
        if (IsTruthy(Evaluate(statement.condition)))
        {
            Execute(statement.thenBranch);
        }
        else if (statement.elseBranch != null)
        {
            Execute(statement.elseBranch);
        }
        return null!;
    }

    //| IVisitStatements<object>
    public object VisitWhileStatement(Statement.WhileStatement statement)
    {
        while (IsTruthy(Evaluate(statement.condition)))
        {
            Execute(statement.body);
        }
        return null!;
    }

    //| IVisitStatements<object>

    public object VisitFunctionStatement(Statement.FunctionStatement statement)
    {
        LoxFunction function = new LoxFunction(statement, environment);
        environment.Define(statement.name.lexeme, function);
        return null!;
    }

    //| IVisitStatements<object>
    public object VisitReturnStatement(Statement.ReturnStatement statement)
    {
        object value = null!;
        if (statement.value != null) value = Evaluate(statement.value);
        throw new Return(value);
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
                if (left is string && right is double) return (string)left + Program.Stringify(right);
                if (left is double && right is string) return Program.Stringify(left) + (string)right;
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

    //| IVisitExpressions<object>
    public object VisitLogicExpression(Expression.Logic expression)
    {
        object left = Evaluate(expression.left);
        if (expression.operatorToken.type == TokenType.OR)
        {
            if (IsTruthy(left)) return left;
        }
        else
        {
            if (!IsTruthy(left)) return left;
        }
        return Evaluate(expression.right);
    }

    //| IVisitExpressions<object>
    public object VisitCallExpression(Expression.Call expression)
    {
        object callee = Evaluate(expression.callee);
        List<object> arguments = new List<object>();
        foreach (Expression argument in expression.arguments)
        {
            arguments.Add(Evaluate(argument));
        }

        if (callee is not ICallLoxFunctions)
        {
            throw new Error.RuntimeError(expression.paren, "Can only call functions and classes.");
        }

        ICallLoxFunctions function = (ICallLoxFunctions)callee;

        if (arguments.Count != function.Arity())
        {
            throw new Error.RuntimeError(expression.paren, $"Expected {function.Arity()} arguments but got {arguments.Count}.");
        }

        return function.Call(this, arguments)!;
    }

    //* NATIVE FUNCTIONS

    public class ClockFunction : ICallLoxFunctions
    {
        public int Arity() => 0;

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            throw new NotImplementedException();
            //return (double)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000.0;
        }

        //public override string ToString() => "<native fn>";
    }

    //* HELPERS

    //| HELPERS
    public void Execute(Statement statement) => statement.Accept(this);
    public object Evaluate(Expression expression) => expression.Accept(this);

    //| HELPER
    public void ExecuteBlock(List<Statement> statements, LoxEnvironment environment)
    {
        LoxEnvironment previous = this.environment;
        try
        {
            this.environment = environment;
            foreach (Statement statement in statements)
            {
                Execute(statement);
            }
        }
        finally
        {
            this.environment = previous;
        }
    }

    //| HELPER
    public void CheckNumberOperands(Token operatorToken, object left, object right)
    {
        if (left is double && right is double) return;
        throw new Error.RuntimeError(operatorToken, "Operands must be numbers.");
    }

    //| HELPER
    public void CheckNumberOperand(Token operatorToken, object operand)
    {
        if (operand is double) return;
        throw new Error.RuntimeError(operatorToken, "Operand must be a number.");
    }

    //| HELPER
    public bool IsEqual(object left, object right)
    {
        if (left == null && right == null) return true;
        if (left == null) return false;
        return left.Equals(right);
    }

    //| HELPER
    public bool IsTruthy(object right)
    {
        if (right == null) return false;
        if (right is bool boolean) return boolean;
        if (right is double number) return number != 0.0;
        return false;
    }

    public void Resolve(Expression expr, int depth)
    {
        locals.Add(expr, depth);
    }
}