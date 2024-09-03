namespace CSLox;

interface ICallLoxFunctions
{
    object Call(Interpreter interpreter, List<object> arguments);
    int Arity();
}