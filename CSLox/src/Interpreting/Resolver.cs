namespace CSLox;

internal class Resolver : Expression.IVisitExpressions<object>, Statement.IVisitStatements<object>
{
    private enum FunctionType
    {
        NONE,
        FUNCTION,
        METHOD,
    }
    
    Interpreter interpreter;
    Stack<Dictionary<string, bool>> scopes;
    private FunctionType currentFunction = FunctionType.NONE;

    public Resolver(Interpreter interpreter)
    {
        this.interpreter = interpreter;
        scopes = new Stack<Dictionary<string, bool>>();
    }

    public void Resolve(List<Statement> statements)
    {
        foreach (Statement statement in statements)
        {
            Resolve(statement);
        }
    }

    //* Expressions

    public object VisitAssignExpression(Expression.Assign expr)
    {
        Resolve(expr.value);
        ResolveLocal(expr, expr.name);
        return null!;
    }

    public object VisitBinaryExpression(Expression.Binary expr)
    {
        Resolve(expr.left);
        Resolve(expr.right);
        return null!;
    }

    public object VisitCallExpression(Expression.Call expr)
    {
        Resolve(expr.callee);

        foreach (Expression argument in expr.arguments)
        {
            Resolve(argument);
        }
        return null!;
    }

    public object VisitGroupingExpression(Expression.Grouping expr)
    {
        Resolve(expr.expression);
        return null!;
    }

    public object VisitLiteralExpression(Expression.Literal expr)
    {
        return null!;
    }

    public object VisitLogicExpression(Expression.Logic expr)
    {
        Resolve(expr.left);
        Resolve(expr.right);
        return null!;
    }

    public object VisitUnaryExpression(Expression.Unary expr)
    {
        Resolve(expr.right);
        return null!;
    }

    public object VisitVariableExpression(Expression.Variable expr)
    {
        Dictionary<string, bool> scope = null!;

        if (scopes.Count() > 0)
        {
            scope = scopes.Peek();

            if (scope == null)
            {
                throw new System.Exception("Scope is null.");
            }

            if (!scope.ContainsKey(expr.name.lexeme))
            {
                Error.Report(new Error.CompileError(expr.name, $"Variable with name '{expr.name.lexeme}' not found."));
            }
            else if (scope[expr.name.lexeme] == false)
            {
                Error.Report(new Error.CompileError(expr.name, $"Can't read local variable in its own initializer."));
            }
        }

        ResolveLocal(expr, expr.name);
        return null!;
    }
    
    public object VisitGetExpression(Expression.Get expr)
    {
        Resolve(expr.obj);
        return null!;
    }
    
    public object VisitSetExpression(Expression.Set expr)
    {
        Resolve(expr.value);
        Resolve(expr.obj);
        return null!;
    }
    
    //* Statements

    public object VisitBlockStatement(Statement.BlockStatement stmt)
    {
        BeginScope();
        Resolve(stmt.statements);
        EndScope();
        return null!;
    }

    public object VisitExpressionStatement(Statement.ExpressionStatement stmt)
    {
        Resolve(stmt.expression);
        return null!;
    }

    public object VisitFunctionStatement(Statement.FunctionStatement stmt)
    {
        Declare(stmt.name);
        Define(stmt.name);

        ResolveFunction(stmt, FunctionType.FUNCTION);
        return null!;
    }

    public object VisitIfStatement(Statement.IfStatement stmt)
    {
        Resolve(stmt.condition);
        Resolve(stmt.thenBranch);
        if (stmt.elseBranch != null) Resolve(stmt.elseBranch);
        return null!;
    }

    public object VisitPrintLineStatement(Statement.PrintLineStatement stmt)
    {
        Resolve(stmt.expression);
        return null!;
    }

    public object VisitPrintStatement(Statement.PrintStatement stmt)
    {
        Resolve(stmt.expression);
        return null!;
    }

    public object VisitReturnStatement(Statement.ReturnStatement stmt)
    {
        if (currentFunction == FunctionType.NONE)
        {
            Error.Report(new Error.CompileError(stmt.keyword, "Can't return from top-level code. Is a return statement outside of a function?"));
        }

        if (stmt.value != null)
        {
            Resolve(stmt.value);
        }

        return null!;
    }

    public object VisitVariableDeclarationStatement(Statement.VariableDeclarationStatement stmt)
    {
        Declare(stmt.name);
        if (stmt.initializer != null)
        {
            Resolve(stmt.initializer);
        }
        Define(stmt.name);
        return null!;
    }

    public object VisitWhileStatement(Statement.WhileStatement stmt)
    {
        Resolve(stmt.condition);
        Resolve(stmt.body);
        return null!;
    }

    public object VisitClassStatement(Statement.ClassStatement stmt)
    {
        Declare(stmt.name);
        Define(stmt.name);
        
        foreach (Statement.FunctionStatement method in stmt.methods)
        {
            FunctionType declaration = FunctionType.METHOD;
            /*if (method.name.lexeme.Equals("init"))
            {
                declaration = FunctionType.FUNCTION;
            }*/
            ResolveFunction(method, declaration);
        }
        
        return null!;
    }

    //| Helper
    private void Declare(Token name)
    {
        if (scopes.Count() == 0) return;

        Dictionary<string, bool> scope = scopes.Peek();
        
        if (scope.ContainsKey(name.lexeme))
        {
            Error.Report(new Error.CompileError(name, $"Variable with name '{name.lexeme}' already declared in this scope."));
        }
        
        scope.Add(name.lexeme, false);
    }
    
    //| Helper
    private void Define(Token name)
    {
        if (scopes.Count() == 0) return;
        scopes.Peek()[name.lexeme] = true;
    }

    //| Helper
    private void ResolveFunction(Statement.FunctionStatement function, FunctionType functionType)
    {
        FunctionType enclosingFunction = currentFunction;
        currentFunction = functionType;
        
        BeginScope();
        foreach (Token param in function.parameters)
        {
            Declare(param);
            Define(param);
        }
        Resolve(function.body);
        EndScope();

        currentFunction = enclosingFunction;
    }

    //| Helper
    private void ResolveLocal(Expression expr, Token name)
    {
        for (int i = scopes.Count() - 1; i >= 0; i--)
        {
            if (scopes.ElementAt(i).ContainsKey(name.lexeme))
            {
                interpreter.Resolve(expr, scopes.Count() - 1 - i);
                return;
            }
        }
    }

    //| Helpers
    void Resolve(Statement statement) => statement.Accept(this);
    void Resolve(Expression expression) => expression.Accept(this);
    void BeginScope() => scopes.Push(new Dictionary<string, bool>());
    void EndScope() => scopes.Pop(); 
}