namespace CSLox;

/*

program        → declaration* EOF ;

declaration    → varDecl | statement ;
varDecl        → "var" IDENTIFIER ( "=" expression )? ";" ;
statement      → exprStmt | printStmt | printLineStmt | block | ifStmt ;

exprStmt       → expression ";" ;
printStmt      → "print" expression ";" ;
printLineStmt  → "printLine" expression ";" ;
block          → "{" declaration* "}" ;
ifStmt         → "if" "(" expression ")" statement ( "else" statement )? ;

expression     → assignment ;
assignment     → IDENTIFIER "=" assignment | logic_or ;

logic_or       → logic_and ( "or" logic_and )* ;
logic_and      → equality ( "and" equality )* ;
equality       → comparison ( ( "!=" | "==" ) comparison )* ;
comparison     → term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
term           → factor ( ( "-" | "+" ) factor )* ;
factor         → unary ( ( "/" | "*" ) unary )* ;
unary          → ( "!" | "-" ) unary | primary ;
primary        → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")" | IDENTIFIER ;

*/

internal class Parser
{
    private List<Token> tokens; 
    private int current = 0;
    
    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }
    
    //| program → declaration* EOF;
    public List<Statement> Parse()
    {
        List<Statement> statements = new List<Statement>();
        while (!IsAtEnd())
        {
            statements.Add(ParseDeclaration());
        }

        return statements;
    }

    //| declaration → varDecl | statement ;
    private Statement ParseDeclaration()
    {
        try
        {
            if (Match(TokenType.VAR)) return ParseVariableDeclaration();
            return ParseStatement();
        }
        catch (Error.ParseError)
        {
            Synchronize();
            return null!;
        }
    }


    //| varDecl → "var" IDENTIFIER( "=" expression )? ";" ;
    private Statement ParseVariableDeclaration()
    {
        Token name = Consume(TokenType.IDENTIFIER, "Expected variable name.");
        Expression? initializer = null;
        if (Match(TokenType.EQUAL))
        {
            initializer = ParseExpression();
        }
        Consume(TokenType.SEMICOLON, "Expected ';' after variable declaration.");
        return new Statement.VariableDeclarationStatement(name, initializer);
    }

    //| statement → exprStmt | printStmt | printLineStmt ;
    Statement ParseStatement()
    {
        try
        {
            if (Match(TokenType.PRINT)) return ParsePrintStatement();
            if (Match(TokenType.PRINTLINE)) return ParsePrintLineStatement();
            if (Match(TokenType.LEFT_BRACE)) return new Statement.BlockStatement(ParseBlock());
            if (Match(TokenType.IF)) return ParseIfStatement();
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
    
    //| block → "{" declaration* "}" ;
    private List<Statement> ParseBlock()
    {
        List<Statement> statements = new List<Statement>();
        while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
        {
            statements.Add(ParseDeclaration());
        }
        Consume(TokenType.RIGHT_BRACE, "Expected '}' after block.");
        return statements;
    }
    
    //| ifStmt → "if" "(" expression ")" statement ( "else" statement )? ;
    
    private Statement ParseIfStatement()
    {
        Consume(TokenType.LEFT_PAREN, "Expected '(' after 'if'.");
        Expression condition = ParseExpression();
        Consume(TokenType.RIGHT_PAREN, "Expected ')' after if condition.");
        
        Statement thenBranch = ParseStatement();
        Statement? elseBranch = null;
        
        if (Match(TokenType.ELSE))
        {
            elseBranch = ParseStatement();
        }
        
        return new Statement.IfStatement(condition, thenBranch, elseBranch);
    }    
        
    //| exprStmt → expression ";" ;
    Statement ParseExpressionStatement()
    {
        Expression expression = ParseExpression();
        Consume(TokenType.SEMICOLON, "Expect ';' after expression.");
        return new Statement.ExpressionStatement(expression);
    }
    
    //| expression → assignment ;
    private Expression ParseExpression()
    {
        return ParseAssign();
    }

    //| assignment → IDENTIFIER "=" assignment | logic_or ;
    private Expression ParseAssign()
    {
        Expression expression = ParseOr();
        
        if (Match(TokenType.EQUAL))
        {
            Token equals = Previous();
            Expression value = ParseAssign();
            
            if (expression is Expression.Variable variable)
            {
                Token name = variable.name;
                return new Expression.Assign(name, value);
            }
            
            ParseError(equals, "Invalid assignment target.");
        }
        
        return expression;
    }    
    
    //| logic_or → logic_and( "or" logic_and )* ;
    private Expression ParseOr()
    {
        Expression expression = ParseAnd();
        
        while (Match(TokenType.OR))
        {
            Token op = Previous();
            Expression right = ParseAnd();
            expression = new Expression.Logic(expression, op, right);
        }
        
        return expression;
    }
    
    //| logic_and → equality( "and" equality )* ;
    private Expression ParseAnd()
    {
        Expression expression = ParseEquality();
        
        while (Match(TokenType.AND))
        {
            Token op = Previous();
            Expression right = ParseEquality();
            expression = new Expression.Logic(expression, op, right);
        }
        
        return expression;
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

        if (Match(TokenType.IDENTIFIER))
        {
            return new Expression.Variable(Previous());
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
}