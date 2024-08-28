namespace Sandbox_Simulator_2024.Scripting;

public class TestExampleScript
{
    public static string test =
"""
### `test` EXAMPLE SCRIPT ###

create action TestAction that does print "Test action called!".

create interface ITestInterface that has:
    bool IsTest,
    action TestAction.
    
create host TestHost that implements ITestInterface that has:
    bool IsTest set to true,
    action TestAction set to print "Test action called!",
    TestAction also .


""";
}