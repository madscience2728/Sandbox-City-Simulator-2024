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
    // while(1 == 1) {
    //  printLine "a";
    // }
    
    a = 0;
    b = 1;
    c = -20;
    
    
    var temp;
    while (a < 89) {
      printLine a;
      temp = a;
      a = b;
      b = temp + b;
    }
    
    
    printLine a + c;
    
    a = 0;
    b = 1;
    
    for (var b = 1; a < 89; b = temp + b) {
      printLine a;
      temp = a;
      a = b;
    }
    printLine a + c; 
    
    printLine "done " + 0;
    
    var p = 3;
    
    fun pizza() {
      printLine "pizza";
      p = p - 1;
      if(p > 0) pizza();
    }
    pizza();
    
    fun sayHi(first, last) {
      printLine "Hi, " + first + " " + last + "!";
      pizza();
    }
    sayHi("Dear", "Reader");
   
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