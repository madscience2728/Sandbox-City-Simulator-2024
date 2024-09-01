namespace CSLox;

using CSLox.Parsing;
using CSLox.Scanning;

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
}