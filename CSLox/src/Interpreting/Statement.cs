namespace CSLox;

// Statements change state... or they produce output.
internal abstract class Statement
{
    public abstract T Accept<T>(IVisitStatements<T> visitor);

    public interface IVisitStatements<T>
    {
        T VisitPrintStatement(PrintStatement stmt);
        T VisitPrintLineStatement(PrintLineStatement stmt);
        T VisitExpressionStatement(ExpressionStatement stmt);
        T VisitVariableDeclarationStatement(VariableDeclarationStatement stmt);
        T VisitBlockStatement(BlockStatement stmt);
        T VisitIfStatement(IfStatement stmt);
        T VisitWhileStatement(WhileStatement stmt);
        T VisitFunctionStatement(FunctionStatement stmt);
        T VisitReturnStatement(ReturnStatement stmt);
    }

    public class PrintStatement : Statement
    {
        public Expression expression;
        
        public PrintStatement(Expression expression)
        {
            this.expression = expression;
        }
        
        public override T Accept<T>(IVisitStatements<T> visitor)
        {
            return visitor.VisitPrintStatement(this);
        }
    }
    
    public class PrintLineStatement : Statement
    {
        public Expression expression;
        
        public PrintLineStatement(Expression expression)
        {
            this.expression = expression;
        }
        
        public override T Accept<T>(IVisitStatements<T> visitor)
        {
            return visitor.VisitPrintLineStatement(this);
        }
    }

    public class ExpressionStatement : Statement
    {
        public Expression expression;
        
        public ExpressionStatement(Expression expression)
        {
            this.expression = expression;
            Console.WriteLine("Expression: " + expression);
        }
        
        public override T Accept<T>(IVisitStatements<T> visitor)
        {
            return visitor.VisitExpressionStatement(this);
        }
    }
    
    public class VariableDeclarationStatement : Statement
    {
        public Token name;
        public Expression? initializer;
        
        public VariableDeclarationStatement(Token name, Expression? initializer)
        {
            this.name = name;
            this.initializer = initializer;
        }
        
        public override T Accept<T>(IVisitStatements<T> visitor)
        {
            return visitor.VisitVariableDeclarationStatement(this);
        }
    }
    
    public class BlockStatement : Statement
    {
        public List<Statement> statements;
        
        public BlockStatement(List<Statement> statements)
        {
            this.statements = statements;
        }
        
        public override T Accept<T>(IVisitStatements<T> visitor)
        {
            return visitor.VisitBlockStatement(this);
        }
    }
    
    public class IfStatement : Statement
    {
        public Expression condition;
        public Statement thenBranch;
        public Statement? elseBranch;
        
        public IfStatement(Expression condition, Statement thenBranch, Statement? elseBranch)
        {
            this.condition = condition;
            this.thenBranch = thenBranch;
            this.elseBranch = elseBranch;
        }
        
        public override T Accept<T>(IVisitStatements<T> visitor)
        {
            return visitor.VisitIfStatement(this);
        }
    }
    
    public class WhileStatement : Statement
    {
        public Expression condition;
        public Statement body;
        
        public WhileStatement(Expression condition, Statement body)
        {
            this.condition = condition;
            this.body = body;
        }
        
        public override T Accept<T>(IVisitStatements<T> visitor)
        {
            return visitor.VisitWhileStatement(this);
        }
    }
    
    public class FunctionStatement : Statement
    {
        public Token name;
        public List<Token> parameters;
        public List<Statement> body;
        
        public FunctionStatement(Token name, List<Token> parameters, List<Statement> body)
        {
            this.name = name;
            this.parameters = parameters;
            this.body = body;
        }
        
        public override T Accept<T>(IVisitStatements<T> visitor)
        {
            return visitor.VisitFunctionStatement(this);
        }
    }
    
    public class ReturnStatement : Statement
    {
        public Token keyword;
        public Expression? value;
        
        public ReturnStatement(Token keyword, Expression? value)
        {
            this.keyword = keyword;
            this.value = value;
        }
        
        public override T Accept<T>(IVisitStatements<T> visitor)
        {
            return visitor.VisitReturnStatement(this);
        }
    }
}