namespace CSLox.Parsing;

using CSLox.Scanning;

/*

program        → statement* EOF ;
statement      → exprStmt | printStmt | printLineStmt ;
exprStmt       → expression ";" ;
printStmt      → "print" expression ";" ;
printLineStmt  → "printLine" expression ";" ;

expression     → equality ;
equality       → comparison ( ( "!=" | "==" ) comparison )* ;
comparison     → term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
term           → factor ( ( "-" | "+" ) factor )* ;
factor         → unary ( ( "/" | "*" ) unary )* ;
unary          → ( "!" | "-" ) unary | primary ;
primary        → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")" ;

*/

internal class Parser
{
    private List<Token> tokens; 
    private int current = 0;
    
    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    //| program → statement* EOF;
    public List<Statement> Parse()
    {
        List<Statement> statements = new List<Statement>();
        while (!IsAtEnd())
        {
            statements.Add(ParseStatement());
        }

        return statements;
    }

    public Expression? ParseSingle()
    {
        try
        {
            return ParseExpression();
        }
        catch (Error.ParseError)
        {
            return null;
        }
    }

    //| Error handling
    public Error.ParseError ParseError(Token token, string message)
    {
        Error.Report(token.line, message);
        return new Error.ParseError(token, message);
    }

    //| Error handling
    private void Synchronize()
    {
        Advance();

        while (!IsAtEnd())
        {
            if (Previous().type == TokenType.SEMICOLON) return;

            switch (Peek().type)
            {
                case TokenType.CLASS:
                case TokenType.FUN:
                case TokenType.VAR:
                case TokenType.FOR:
                case TokenType.IF:
                case TokenType.WHILE:
                case TokenType.PRINT:
                case TokenType.RETURN:
                    return;
            }

            Advance();
        }
    }
    
    //| statement → exprStmt | printStmt | printLineStmt ;
    Statement ParseStatement()
    {
        try
        {
            if (Match(TokenType.PRINT)) return ParsePrintStatement();
            if (Match(TokenType.PRINTLINE)) return ParsePrintLineStatement();
            return ParseExpressionStatement();
        }
        catch (Error.ParseError)
        {
            Synchronize();
            return null!;
        }
    }

    //| printStmt → "print" expression ";" ;
    Statement ParsePrintStatement()
    { 
        Expression expression = ParseExpression();
        Consume(TokenType.SEMICOLON, "Expected ';' after value.");
        return new Statement.PrintStatement(expression);
    }
    
    //| printLineStmt → "printLine" expression ";" ;
    Statement ParsePrintLineStatement()
    {
        Expression expression = ParseExpression();
        Consume(TokenType.SEMICOLON, "Expected ';' after value.");
        return new Statement.PrintLineStatement(expression);
    }

    //| exprStmt → expression ";" ;
    Statement ParseExpressionStatement()
    {
        Expression expression = ParseExpression();
        Consume(TokenType.SEMICOLON, "Expect ';' after expression.");
        return new Statement.ExpressionStatement(expression);
    }
    
    //| expression → equality ;
    private Expression ParseExpression()
    {
        return ParseEquality();
    }

    //| equality → comparison(( "!=" | "==" ) comparison )* ;
    private Expression ParseEquality()
    {
        Expression expression = ParseComparison();
        
        while(Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
        {
            Token op = Previous();
            Expression right = ParseComparison();
            expression = new Expression.Binary(expression, op, right);
        }
        
        return expression;
    }
    
    //| comparison → term(( ">" | ">=" | "<" | "<=" ) term )* ;
    private Expression ParseComparison()
    {
        Expression expression = ParseTerm();
        
        while(Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
        {
            Token op = Previous();
            Expression right = ParseTerm();
            expression = new Expression.Binary(expression, op, right);
        }
        
        return expression;
    }
    
    //| term → factor(( "-" | "+" ) factor )* ;
    private Expression ParseTerm()
    {
        Expression expression = ParseFactor();
        
        while(Match(TokenType.MINUS, TokenType.PLUS))
        {
            Token op = Previous();
            Expression right = ParseFactor();
            expression = new Expression.Binary(expression, op, right);
        }
        
        return expression;
    }
    
    //| factor → unary(( "/" | "*" ) unary )* ;
    private Expression ParseFactor()
    {
        Expression expression = ParseUnary();
        
        while(Match(TokenType.SLASH, TokenType.STAR))
        {
            Token op = Previous();
            Expression right = ParseUnary();
            expression = new Expression.Binary(expression, op, right);
        }
        
        return expression;
    }
    
    //| unary → ( "!" | "-" ) unary | primary ;
    private Expression ParseUnary()
    {
        if (Match(TokenType.BANG, TokenType.MINUS))
        {
            Token op = Previous();
            Expression right = ParseUnary();
            return new Expression.Unary(op, right);
        }
        
        return ParsePrimary();
    }

    //| primary → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")" ;
    private Expression ParsePrimary()
    {
        if (Match(TokenType.FALSE)) return new Expression.Literal(false);
        if (Match(TokenType.TRUE)) return new Expression.Literal(true);
        if (Match(TokenType.NIL)) return new Expression.Literal(null);

        if (Match(TokenType.NUMBER, TokenType.STRING))
        {
            return new Expression.Literal(Previous().literal);
        }

        if (Match(TokenType.LEFT_PAREN))
        {
            Expression expression = ParseExpression();
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression. Are you missing a closing parenthesis?");
            return new Expression.Grouping(expression);
        }

        throw ParseError(Peek(), $"Expected expression, found {Peek()}.");
    }

    //| HELPER
    private Token Consume(TokenType type, string message)
    {
        if (Check(type)) return Advance();
        throw ParseError(Previous(), message);
    }

    //| HELPER
    private bool Match(params TokenType[] types)
    {
        foreach(TokenType type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }

        return false;
    }

    //| HELPER'S HELPER
    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().type == type;
    }

    //| HELPER
    private Token Advance()
    {
        if (!IsAtEnd()) current++;
        return Previous();
    }

    //| HELPER
    private Token PeekPeek()
    {
        if(IsAtEnd()) return new Token(TokenType.EOF, "", null, 0);
        return tokens[current + 1];
    }

    //| HELPERS
    private bool IsAtEnd() => Peek().type == TokenType.EOF;
    private Token Peek() => tokens[current];
    private Token Previous() => tokens[current - 1];
}