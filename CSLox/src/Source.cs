namespace CSLox;

public class Source
{

    public static readonly string source =
    """
    
    
    
    // this is a comment
    printLine "Hello world";
    var a = 10;
    var b = 1;
    printLine a + b;
    
    fun bad()
    {
      a = 20;
      printLine a + b;
      
      var a = 30; // ignored
      printLine a + b; 
      
      return 41;
    }
    var c = bad();
    printLine c;
    
    fun test()
    {
      var romeo = "Romeo";
      var juliet = "Juliet";
      romeo = "Montague";
      printLine romeo + " " + juliet;
    }
    
    class DevonshireCream {
    }
    printLine DevonshireCream;
    var dc = DevonshireCream;
    
    
    """;

    public static readonly string sourceOld =


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
    
    fun count(n) {
      while (n < 100) {
        if (n == 3) return n; // <--
        print n;
        n = n + 1;
      }
    }
    count(1);
    
    fun fib(n) {
      if(n <= 1) return n;
      return fib(n - 1) + fib(n - 2);
    }
    
    for (var i = 0; i < 20; i = i + 1) {
      printLine fib(i);
    }
   
    fun makeCounter() {
      var i = 0;
      fun count() {
        i = i + 1;
        printLine i;
      }
      return count;
    }

    var counter = makeCounter();
    counter(); // "1".
    counter(); // "2".
   
   
    var a = "global";
    if(true)
    {
      fun showA() {
        printLine a;
      }
    
      showA();
      var a = "block";
      showA();
      // a = a;
    }
    printLine a;
    
    printLine "";
    
    a = "global";
    if(true)
    {
      a = "block";
      a = a;
    }
    printLine a;
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