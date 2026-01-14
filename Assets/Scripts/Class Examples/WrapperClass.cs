using System;
using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    // Uses WrapperClass as the delegate
    public event Action<WrapperClass> exampleEvent; 

    private int _newVar;
    private int _newVar2;
    private int _newVar3;
  
    // We only need to pass the class itself as a single argument, and then all its fields can be accessed through dot notation
    private void ExampleMethod(WrapperClass exampleClass)
    {
        _newVar = exampleClass.arg1;
        _newVar2 = exampleClass.arg2;
        _newVar3 = exampleClass.arg3;
    }
  
    
    private void InvokeMethod()
    {
        // This is where I set the wrapper variables
        var args = new WrapperClass(
            x: 10,
            y: 20,
            z: 30
            );
        
        exampleEvent?.Invoke(args);
    }
}


// This class acts as the wrapper, and can hold an essentially infinite number of variables, bypassing the 16 delegate limit set by event Actions
public class WrapperClass
{
    public int arg1;
    public int arg2;
    public int arg3;
    
    // This struct is necessary for defining the wrapper fields
    public WrapperClass(int x, int y, int z)
    {
        arg1 = x;
        arg2 = y;
        arg3 = z;
    }
    
}

