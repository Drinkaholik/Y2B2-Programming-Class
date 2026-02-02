using UnityEngine;

// Moves bullet, handles collisions, and checks lifetime
// Takes all its stats from scriptableObjects

public class Bullet : MonoBehaviour
{
    
    private float _timeLived;

    [HideInInspector] public Gun gun;

    public TrailRenderer trail;

    private Renderer _rend;
    public Renderer rend
    {
        get
        {
            if (_rend == null) _rend = GetComponent<Renderer>();
            return _rend;
        }
    }

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        _rend = GetComponent<Renderer>();
    }
    
    
    void Update()
    {
        _timeLived += Time.deltaTime; // Needed for sidewind movement and lifetime check
        
        CheckLifetime();
        gun.moveType.Move(gameObject, gun.shootType.speed, _timeLived);
        CollisionCheck();
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


    void CollisionCheck()
    {
        var rayDir = transform.TransformDirection(gun.moveType.moveDir);
        var rayLength = gun.shootType.speed * Time.deltaTime;
        
        //Debug.DrawRay(transform.position, rayDir * (rayLength * 10), Color.red, 1f);
        
        if (Physics.Raycast(transform.position, rayDir, out RaycastHit hit, rayLength))
        {
            transform.position = hit.point;
            
            OnHit(hit.collider);
        }
        
    }
    
    
    // On hit
    void OnHit(Collider other)
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
            gun.elementType.ApplyEffect(other);
        }
        
        ReturnToPool();
        
    }
    
    
    
    
// Class ends here
}

