using UnityEngine;

// Holds info and methods for bullet movement
// All movement types are mutually exclusive
// Base is instantiable
    
[CreateAssetMenu(fileName = "MovementType", menuName = "Scriptable Objects/MovementType")]
public class MovementType : ScriptableObject
{
    
    public virtual void Move(GameObject go, float speed)
    {
        go.transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }
}



[CreateAssetMenu(fileName = "SideWinder", menuName = "Scriptable Objects/SideWinder")]
public class SideWinder : MovementType
{
    [Header("Oscillation")] 
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;

    private float _lifetime;

    
    private enum Orientation
    {
        Horizontal,
        Vertical
    }
    
    [SerializeField] private Orientation orientation;
    private Vector3 _direction;
    

    public override void Move(GameObject go, float speed)
    {
        // Handle orientation
        switch (orientation)
        {
            case Orientation.Horizontal:
                _direction = Vector3.right;
                break;
            case Orientation.Vertical:
                _direction = Vector3.up;
                break;
        }
        
        // Handle oscillation
        _lifetime += Time.deltaTime;
        var sin = amplitude * Mathf.Sin((2 * Mathf.PI * _lifetime * frequency) + (0.5f * Mathf.PI));
        
        // Movement
        var moveDir = (Vector3.forward + (_direction * sin)).normalized;
        go.transform.Translate( moveDir * (speed * Time.deltaTime));
    }
}



public class Spiral : MovementType
{
    
    
    
    
}