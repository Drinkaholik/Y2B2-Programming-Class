using UnityEngine;

// Describe class function here

public class PlayerController : MonoBehaviour
{


    [SerializeField] private Transform spawnPoint;
    
    // Component references
    [SerializeField] private Health health;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerLook look;


    void Start()
    {
        health.OnDeath += Die;

    }



    private void Die()
    {
        
        transform.position = spawnPoint.position;
        
        
    }
    
    
    
    
    
// Class ends here
}
