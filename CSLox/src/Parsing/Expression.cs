namespace CSLox;

internal abstract class Expression
{
    public interface IVisitExpressions<T>
    {
        T VisitBinaryExpression(Binary expr);
        T VisitGroupingExpression(Grouping expr);
        T VisitLiteralExpression(Literal expr);
        T VisitUnaryExpression(Unary expr);
        T VisitVariableExpression(Variable expr);
    }
    
    public abstract T Accept<T>(IVisitExpressions<T> visitor);
    
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
        
        public override T Accept<T>(IVisitExpressions<T> visitor)
        {
            return visitor.VisitBinaryExpression(this);
        }
    }
    
    public class Grouping : Expression
    {
        public Expression expression { get; private set; }

        public Grouping(Expression expression)
        {
            this.expression = expression;
        }
        
        public override T Accept<T>(IVisitExpressions<T> visitor)
        {
            return visitor.VisitGroupingExpression(this);
        }
    }
    
    public class Literal : Expression
    {
        public object value { get; private set; }

        public Literal(object? value)
        {
            this.value = value!;
        }
        
        public override T Accept<T>(IVisitExpressions<T> visitor)
        {
            return visitor.VisitLiteralExpression(this);
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
        
        public override T Accept<T>(IVisitExpressions<T> visitor)
        {
            return visitor.VisitUnaryExpression(this);
        }
    }
    
    public class Variable : Expression
    {
        public Token name { get; private set; }

        public Variable(Token name)
        {
            this.name = name;
        }
        
        public override T Accept<T>(IVisitExpressions<T> visitor)
        {
            return visitor.VisitVariableExpression(this);
        }
    }
}