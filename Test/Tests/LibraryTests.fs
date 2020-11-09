module Tests

open NUnit.Framework
open Example

[<TestFixture>]
type ExampleTests () = 

    [<Test>]
    member this.functest () = 
        Assert.AreEqual (func (), ()) 