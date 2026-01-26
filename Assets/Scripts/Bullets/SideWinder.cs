using UnityEngine;

// Describe class function here

public class SideWinder : Bullet
{

    [SerializeField] private float curve;
    
    
    void Update()
    {
        
        
    }

    protected override void Move()
    {

        var moveDir = 0;
        transform.Translate(Vector3.forward * (_bullet.Speed * Time.deltaTime));
    }
    
    
// Class ends here
}
