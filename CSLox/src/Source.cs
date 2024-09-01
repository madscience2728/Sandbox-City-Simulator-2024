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
    """;

    /*
    
    Outputs:
        >> Hello world
        >> 6.2
        >> one two
        >> True
    
    */









    //     public static readonly string source =
    // """
    // // this is a comment
    // (( )){} // grouping stuff
    //     !*+-/=<> <= == >= // operators
    //     "Hello world";
    // 123.321;
    // and
    // class
    // else
    // false
    // for
    // fun
    // if
    // nil
    // or
    // print
    // return
    // super
    // this
    // true
    // var
    // while
    // bleep bloop;
    // bloop bleep
    // """;
}