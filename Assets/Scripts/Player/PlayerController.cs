using UnityEngine;

// Describe class function here

public class PlayerController : MonoBehaviour, IDamageable
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
        
        //transform.position = spawnPoint.position;
        
        
    }

    public void TakeDamage(int damage)
    {
        
    }

    public void OnDestroy()
    {
        
    }
    
    
    
    
    
// Class ends here
}
