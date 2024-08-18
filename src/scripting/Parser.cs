namespace Sandbox_Simulator_2024;

using Network.Core;
using Sandbox_Simulator_2024.Scripting;

public class Parser
{
    Dictionary<string, List<Token>> identifiers = new();
    Dictionary<string, string> names = new();
    Dictionary<string, List<string>> interfaces = new();
    List<string> PacketIdentifiers = new();
    List<string> InterfaceIdentifiers = new();
    List<string> RouterIdentifiers = new();
    List<string> HostIdentifiers = new();
    List<string> ListIdentifiers = new();
    List<string> IdentifierIdentifiers = new();

    Dictionary<string, List<Node>> lists = new();
    Dictionary<string, List<ScriptableRouter>> routers = new();
    Dictionary<string, List<ScriptableHost>> hosts = new();


    public void Parse(string script)
    {
        var tokens = Tokenizer.Tokenize(script);
        Console.WriteLine("Found " + tokens.Count + " tokens");

        // Collect next line by injesting tokens until a delimiter token is found
        
        IterateExpressions(tokens, ParseExpression);
    }
    
    public void IterateExpressions(IEnumerable<Token> tokens, Action<List<Token>> Parse)
    {
        var lineTokens = new List<Token>();
        foreach (var token in tokens)
        {
            if (token.Type == Token.TokenType.Delimiter)
            {
                Parse(lineTokens);
                lineTokens.Clear();
                Console.WriteLine();
            }
            else if (token.Type == Token.TokenType.NewLine)
            {
                // We could do something here, but it's more helpful to see it how the parser sees it
            }
            else if (token.Type == Token.TokenType.Ignored)
            {
                // We could do something here, but it's more helpful to see it how the parser sees it
            }
            else if (token.Type == Token.TokenType.Whitespace)
            {
                // We could do something here, but it's more helpful to see it how the parser sees it
            }
            else if (token.Type == Token.TokenType.Comment)
            {
                // We could do something here, but it's more helpful to see it how the parser sees it
            }
            else
            {
                lineTokens.Add(token);
            }
        }
    }

    public void ParseExpression(IEnumerable<Token> tokens)
    {
        ValidateAndPrintExpression(tokens);

        int Count = tokens.Count();
        if (Count == 3) ParseAs3Tokens(tokens);
        else
        {
            Token firstToken = tokens.First();
            Token secondToken = tokens.Skip(1).First();
            Token thirdToken = tokens.Last();
            if (secondToken.Value == "is") ParseAsIsStatement(tokens);
            else if (secondToken.Value == "set") ParseAsSetStatement(tokens);
            else if (secondToken.Value == "has") ParseAsHasStatement(tokens);
            else if (secondToken.Value == "implements") ParseAsImplementsStatement(tokens);
            else throw new Exception($"Expected is, set, has, or implements, got {secondToken.Value} at line {secondToken.SourceLineNumber}");
        }
    }

    void ParseAsImplementsStatement(IEnumerable<Token> tokens)
    {
        Console.Write("  >>  Implementing imeplements");
        // TODO - Implement create
        Console.Write("... later");
    }

    void ParseAsHasStatement(IEnumerable<Token> tokens)
    {
        Console.Write("  >>  Implementing has");
        // TODO - Implement create
        Console.Write("... later");
    }

    void ParseAsSetStatement(IEnumerable<Token> tokens)
    {
        // Easy, we only sdupport "set name"

        if (tokens.Count() != 4) throw new Exception("When using set, 4 token are expected <identifier> set name <value>, got " + tokens.Count() + " number of tokens");

        Token identifier = tokens.First();
        Token set = tokens.Skip(1).First();
        Token name = tokens.Skip(2).First();
        Token value = tokens.Last();

        // Check to make sure we have the right tokens
        if (set.Value != "set") throw new Exception("Expected set, got " + set.Value + " at line " + set.SourceLineNumber);
        if (name.Value != "name") throw new Exception("Expected name after set, got " + name.Value + " at line " + name.SourceLineNumber);

        // Now we can set the name
        string newName = value.Value;
        if (newName.Contains("{name generator}")) newName = newName.Replace("{name generator}", NameGenerator.GenerateName());
        Console.Write($"  >>  Setting name of {identifier.Value} to {newName}");
        names.Add(identifier.Value, newName);
    }

    void ParseAsIsStatement(IEnumerable<Token> tokens)
    {
        string typeToke = tokens.Skip(2).First().Value;
        if (typeToke == "interface") ParseAsInterface(tokens);
        else if (typeToke == "create") ParseAsCreate(tokens);
        else throw new Exception($"Expected interface or create, got {typeToke} at line {tokens.Skip(2).First().SourceLineNumber}");
    }

    private void ParseAsCreate(IEnumerable<Token> tokens)
    {
        Console.Write("  >>  Implementing create");
        // TODO - Implement create
        Console.Write("... later");
    }

    private void ParseAsInterface(IEnumerable<Token> tokens)
    {
        Console.Write("  >>  Implementing interface");
        // TODO - Implement interface
        Console.Write("... later");
    }

    void ParseAs3Tokens(IEnumerable<Token> tokens)
    {
        // This means whe either have a...
        // 1. Identifier, Keyword, Keyword
        // 2. Identifier, Keyword, Identifier

        Token firstToken = tokens.First();
        Token secondToken = tokens.Skip(1).First();
        Token thirdToken = tokens.Last();

        if (firstToken.Type != Token.TokenType.Identifier) throw new Exception($"Expected identifier, got type {firstToken.Type} with {firstToken.Value} at line {firstToken.SourceLineNumber}");
        if (secondToken.Type != Token.TokenType.Keyword) throw new Exception($"Expected keyword, got type {secondToken.Type} with {secondToken.Value} at line {secondToken.SourceLineNumber}");
        if (thirdToken.Type == Token.TokenType.Identifier ^ thirdToken.Type != Token.TokenType.Keyword) throw new Exception($"Expected identifier or keyword, got type {thirdToken.Type} with {thirdToken.Value} at line {thirdToken.SourceLineNumber}");

        if (secondToken.Value == "is")
        {
            if (thirdToken.Type == Token.TokenType.Keyword)
            {
                // Now make record of the identifier
                if (thirdToken.Value == "router")
                {
                    RouterIdentifiers.Add(firstToken.Value);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"  >>  {firstToken.Value} has been registered has a router");
                }
                else if (thirdToken.Value == "host")
                {
                    HostIdentifiers.Add(firstToken.Value);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"  >>  {firstToken.Value} has been registered has a host");
                }
                else if (thirdToken.Value == "list")
                {
                    ListIdentifiers.Add(firstToken.Value);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"  >>  {firstToken.Value} has been registered has a list");
                }
                else if (thirdToken.Value == "interface")
                {
                    InterfaceIdentifiers.Add(firstToken.Value);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"  >>  {firstToken.Value} has been registered has an interface");
                }
                else if (thirdToken.Value == "packet")
                {
                    PacketIdentifiers.Add(firstToken.Value);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"  >>  {firstToken.Value} has been registered has a packet");
                }
                else throw new Exception($"Expected router, host, list, interface, or packet, got {thirdToken.Value} at line {thirdToken.SourceLineNumber}");
            }
            else if (thirdToken.Type == Token.TokenType.Identifier)
            {
                IdentifierIdentifiers.Add(firstToken.Value);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"  >>  {firstToken.Value} has been registered has an identifier that derives from {thirdToken.Value}");
            }
            else throw new Exception($"Expected keyword or identifier, got {thirdToken.Value} at line {thirdToken.SourceLineNumber}");
        }
        else if (secondToken.Value == "interfaces")
        {
            if (interfaces.ContainsKey(firstToken.Value)) interfaces[firstToken.Value].Add(thirdToken.Value);
            else interfaces.Add(firstToken.Value, new List<string>() { thirdToken.Value });
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"  >>  {firstToken.Value} has been registered has an interface that derives from {thirdToken.Value}");
        }
        else if (secondToken.Value == "includes")
        {
            if (thirdToken.Type != Token.TokenType.Identifier) throw new Exception($"Expected identifier after the includes keywords, got type {thirdToken.Type} with {thirdToken.Value} at line {thirdToken.SourceLineNumber}");

            // This is concactenation of lists, so first we make sure we have two lists
            if (!ListIdentifiers.Contains(firstToken.Value)) throw new Exception($"Expected list identifier, got {firstToken.Value} at line {firstToken.SourceLineNumber}");
            if (!ListIdentifiers.Contains(thirdToken.Value)) throw new Exception($"Expected list identifier, got {thirdToken.Value} at line {thirdToken.SourceLineNumber}");

            // Now we can do the concactenation
            List<Node> firstList = lists[firstToken.Value];
            List<Node> secondList = lists[thirdToken.Value];
            lists[firstToken.Value] = firstList.Concat(secondList).ToList();
            
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"  >>  Concatenating {firstToken.Value} and {thirdToken.Value}");
        }

    }

    void ValidateAndPrintExpression(IEnumerable<Token> tokens)
    {
        // Check for empty lines
        int Count = tokens.Count();
        if (Count == 0) throw new Exception("Empty line found. You may have a stray delimiter token (a period '.'), possibly on or before line" + tokens.First().SourceLineNumber);
        if (Count == 1) throw new Exception("Expected more than one token, only got: " + tokens.First().Value + " on line " + tokens.First().SourceLineNumber);
        if (Count == 2) throw new Exception("You seem to be missing token on or after line " + tokens.First().SourceLineNumber);

        foreach (var token in tokens)
        {
            bool breakFlag = false;
            bool errorFlag = false;
            Console.BackgroundColor = ConsoleColor.Black;

            switch (token.Type)
            {
                case Token.TokenType.Delimiter:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{token.Value} ");
                    breakFlag = true; // should be last token anyway, so this is redudndant
                    break;
                case Token.TokenType.Keyword:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write($"{token.Value} ");
                    break;
                case Token.TokenType.Identifier:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"{token.Value} ");
                    if (!identifiers.ContainsKey(token.Value)) identifiers[token.Value] = new List<Token>() { token };
                    else identifiers[token.Value].Add(token);
                    break;
                case Token.TokenType.Operator:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{token.Value} ");
                    break;
                case Token.TokenType.Literal:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{token.Value} ");
                    break;
                case Token.TokenType.String:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write($"{token.Value} ");
                    break;
                case Token.TokenType.Comment:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"{token.Value} ");
                    break;
                case Token.TokenType.Whitespace:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{token.Value} ");
                    break;
                case Token.TokenType.NewLine:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    break;
                case Token.TokenType.Unknown:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    breakFlag = true;
                    errorFlag = true;
                    break;
                case Token.TokenType.Ignored:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"{token.Value} ");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"What the heck is this? {token.Value}");
                    breakFlag = true;
                    errorFlag = true;
                    break;
            }

            if (breakFlag) break;
            if (errorFlag) throw new Exception("Error parsing unknown token: " + token.Value);
        }

        Console.ResetColor();
    }
}