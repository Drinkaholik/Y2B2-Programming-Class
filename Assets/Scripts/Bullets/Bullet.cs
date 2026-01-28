using UnityEngine;

// Moves bullet, handles collisions, and checks lifetime
// Takes all its stats from scriptableObjects

public class Bullet : MonoBehaviour
{
    
    
    private float _timeLived;
    
    
    
    void Update()
    {
        CheckLifetime();
        PickupHandler.MovementType.Move(gameObject, PickupHandler.ShootType.speed);
    }
    
    // Destroy if in scene for too long
    void CheckLifetime()
    {
        _timeLived += Time.deltaTime;

        if (_timeLived >= PickupHandler.ShootType.lifetime)
        {
            Destroy(gameObject);
        }
        
    }

    // On hit
    void OnTriggerEnter(Collider other)
    {
        // Apply damage
        PickupHandler.ShootType.OnHit(other);
        
        // Apply effects
        foreach (EffectType fx in PickupHandler.BulletEffects)
        {
            fx.OnHit(gameObject, other);
        }
        
        // Apply elemental effect
        if (PickupHandler.ElementType != null)
        {
            PickupHandler.ElementType.ApplyEffect(gameObject, other);
        }
        
        // Destroy
        Destroy(gameObject);
    }
    
    
    
    
// Class ends here
}

