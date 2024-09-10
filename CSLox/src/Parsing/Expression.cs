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
        T VisitAssignExpression(Assign expr);
        T VisitLogicExpression(Logic expr);
        T VisitCallExpression(Call expr);
        T VisitGetExpression(Get expr);
        T VisitSetExpression(Set expr);
        T VisitThisExpression(This expr);
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
    
    public class Assign : Expression
    {
        public Token name { get; private set; }
        public Expression value { get; private set; }

        public Assign(Token name, Expression value)
        {
            this.name = name;
            this.value = value;
        }
        
        public override T Accept<T>(IVisitExpressions<T> visitor)
        {
            return visitor.VisitAssignExpression(this);
        }
    }
    
    public class Logic : Expression
    {
        public Expression left { get; private set; }
        public Token operatorToken { get; private set; }
        public Expression right { get; private set; }

        public Logic(Expression left, Token operatorToken, Expression right)
        {
            this.left = left;
            this.operatorToken = operatorToken;
            this.right = right;
        }
        
        public override T Accept<T>(IVisitExpressions<T> visitor)
        {
            return visitor.VisitLogicExpression(this);
        }
    }
    
    public class Call : Expression
    {
        public Expression callee { get; private set; }
        public Token paren { get; private set; }
        public List<Expression> arguments { get; private set; }

        public Call(Expression callee, Token paren, List<Expression> arguments)
        {
            this.callee = callee;
            this.paren = paren;
            this.arguments = arguments;
        }
        
        public override T Accept<T>(IVisitExpressions<T> visitor)
        {
            return visitor.VisitCallExpression(this);
        }
    }
    
    public class Get : Expression
    {
        public Expression obj { get; private set; }
        public Token name { get; private set; }

        public Get(Expression obj, Token name)
        {
            this.obj = obj;
            this.name = name;
        }
        
        public override T Accept<T>(IVisitExpressions<T> visitor)
        {
            return visitor.VisitGetExpression(this);
        }
    }
    
    public class Set : Expression
    {
        public Expression obj { get; private set; }
        public Token name { get; private set; }
        public Expression value { get; private set; }

        public Set(Expression obj, Token name, Expression value)
        {
            this.obj = obj;
            this.name = name;
            this.value = value;
        }
        
        public override T Accept<T>(IVisitExpressions<T> visitor)
        {
            return visitor.VisitSetExpression(this);
        }
    }
    
    public class This : Expression
    {
        public Token keyword { get; private set; }

        public This(Token keyword)
        {
            this.keyword = keyword;
        }
        
        public override T Accept<T>(IVisitExpressions<T> visitor)
        {
            return visitor.VisitThisExpression(this);
        }
    }
}