namespace CSLox.Parsing;
using CSLox.Scanning;

internal abstract class Expression
{
    public interface IVisitor<T>
    {
        T VisitBinaryExpr(Binary expr);
        T VisitGroupingExpr(Grouping expr);
        T VisitLiteralExpr(Literal expr);
        T VisitUnaryExpr(Unary expr);
    }
    
    public abstract T Accept<T>(IVisitor<T> visitor);
    
    public class Binary : Expression
    {
        public Expression left { get; private set; }
        public Token operatorToken { get; private set; }
        public Expression right { get; private set; }

        public Binary(Expression left, Token operatorToken, Expression right)
        {
            this.left = left;
            this.operatorToken = operatorToken;
            this.right = right;
        }
        
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }
    
    public class Grouping : Expression
    {
        public Expression expression { get; private set; }

        public Grouping(Expression expression)
        {
            this.expression = expression;
        }
        
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }
    }
    
    public class Literal : Expression
    {
        public object value { get; private set; }

        public Literal(object? value)
        {
            this.value = value!;
        }
        
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }
    
    public class Unary : Expression
    {
        public Token operatorToken { get; private set; }
        public Expression right { get; private set; }

        public Unary(Token operatorToken, Expression right)
        {
            this.operatorToken = operatorToken;
            this.right = right;
        }
        
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }
}