namespace CSLox;

public class Source
{

    public static readonly string source =

    """
    // this is a comment
    printLine "Hello world";
    printLine (1 + 2 * 3 - 4 / 5);
    printLine "one" + " " + "two";
    printLine 1 < 2;
    var a = 1;
    var b = 2;
    printLine a + b;
    var a = "first";
    printLine a; // "first".
    var a = "second";
    printLine a; // "second".
    a = 1;
    printLine a; // "1".
    printLine a = 2; // "2".
    """;

    /*

    Outputs:
    >> Hello world
    >> 6.2
    >> one two
    >> True
    >> 3

    */

}