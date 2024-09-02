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
    var a = "global a";
    var b = "global b";
    var c = "global c";
    {
      var a = "outer a";
      var b = "outer b";
      {
        var a = "inner a";
        printLine a;
        printLine b;
        printLine c;
      }
      printLine "";
      printLine a;
      printLine b;
      printLine c;
    }
    printLine "";
    printLine a;
    printLine b;
    printLine c;
    if(1 == 1) {
      printLine "true";
    } else {
      printLine "false";
    }
    if(1 == 2) {
      printLine "true";
    } else {
      printLine "false";
    }
    if(1 == 2 or 1 == 1) {
      printLine "true";
    } else {
      printLine "false";
    }
    if(1 == 2 and 1 == 1) {
      printLine "true";
    } else {
      printLine "false";
    }
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