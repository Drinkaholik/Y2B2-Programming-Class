using UnityEngine;

// Moves bullet, handles collisions, and checks lifetime
// Takes all its stats from scriptableObjects

public class Bullet : MonoBehaviour
{
    
    
    private float _timeLived;

    [HideInInspector] public Gun gun;
    
    
    void Update()
    {
        _timeLived += Time.deltaTime; // Needed for sidewind movement and lifetime check
        
        CheckLifetime();
        gun.moveType.Move(gameObject, gun.shootType.speed, _timeLived);
    }
    
    

    // On hit
    void OnTriggerEnter(Collider other)
    {
        // Apply damage
        gun.shootType.OnHit(other);
        
        // Apply effects
        foreach (EffectType fx in gun.effects)
        {
            fx.OnHit(gameObject, other);
        }
        
        
        // Apply elemental effect
        if (gun.elementType != null)
        {
            gun.elementType.ApplyEffect(gameObject, other);
        }
        
        ReturnToPool();
        
    }
    
    // Despawn if in scene for too long
    private void CheckLifetime()
    {
        if (_timeLived >= gun.shootType.lifetime)
        {
            _timeLived = 0;
            ReturnToPool();
        }
        
    }

    void ReturnToPool()
    {
        gun.bulletPool.Return(this);
    }
    
    
    
    
// Class ends here
}

