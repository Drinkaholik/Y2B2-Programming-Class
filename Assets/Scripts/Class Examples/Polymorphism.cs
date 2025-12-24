
// Describe class function here


public class MyList<T>
{
    private T[] ts;
}

public interface IShape
{
    void DrawShape();
}

public abstract class Shape
{

    // public virtual void DrawShape() // virtual defines base behaviour
    // {
    //     // Put functionality here that will be shared across all DrawShape() methods
    //     
    // }

    public abstract void DrawShape();

}

public class Triangle : Shape
{

    public override void DrawShape()
    {
        // Put code here that specific to drawing a triangular shape
        
    }
    
}





