using UnityEngine;

// Holds data and methods for bullet movement
// All movement types are mutually exclusive
    
[CreateAssetMenu(fileName = "MovementType", menuName = "Scriptable Objects/MovementType")]
public class MoveType : ModifierType
{
    
    private float _lifetime;
    [HideInInspector] public Vector3 moveDir;

    public enum MoveBehaviour
    {
        Default,
        Sidewind,
        Spiral
    }
    
    public MoveBehaviour behaviour;
    
    
    [Header("Sidewind Stats")] 
    [HideInInspector] [SerializeField] private float amplitude;
    [HideInInspector] [SerializeField] private float frequency;

    // Change between horizontal and vertical
    private enum Orientation { Horizontal, Vertical }
    [HideInInspector] [SerializeField] private Orientation orientation;
    private Vector3 _direction;
    
    
    public void Move(GameObject bullet, float speed, float timeLived)
    {
        switch (behaviour)
        {
            case MoveBehaviour.Default:
                DefaultMove(bullet, speed);
                
                break;
            case MoveBehaviour.Sidewind:
                SidewindMove(bullet, speed, timeLived);
                
                break;
            case MoveBehaviour.Spiral:
                SpiralMove(bullet, speed);
                
                break;
        }
    }

    private void DefaultMove(GameObject bullet, float speed)
    {
        moveDir = bullet.transform.forward;
        bullet.transform.Translate(moveDir * (speed * Time.deltaTime), Space.World);
    }

    private void SidewindMove(GameObject bullet, float speed, float timeLived)
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
        var sin = amplitude * Mathf.Sin((2 * Mathf.PI * timeLived * frequency) + (0.5f * Mathf.PI));
        
        // Apply movement
        moveDir = (bullet.transform.forward + (_direction * sin)).normalized;
        bullet.transform.Translate( moveDir * (speed * Time.deltaTime),  Space.World);
        
    }

    private void SpiralMove(GameObject bullet, float speed)
    {
        
        
    }
    
    
}

