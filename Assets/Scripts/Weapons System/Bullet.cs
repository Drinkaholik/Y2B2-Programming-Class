using System.Collections.Generic;
using UnityEngine;

// Moves bullet, handles collisions, and checks lifetime
// Takes all its stats from scriptableObjects

public class Bullet : MonoBehaviour
{
    
    private float _timeLived;

    [HideInInspector] public ShootType shootType;
    [HideInInspector] public MoveType moveType;
    public List<EffectType> effects;
    [HideInInspector] public ElementType elementType;

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
        moveType.Move(gameObject, shootType.speed, _timeLived);
        CollisionCheck();
    }
    
    

    
    
    // Despawn if in scene for too long
    private void CheckLifetime()
    {
        if (_timeLived >= shootType.lifetime)
        {
            _timeLived = 0;
            ReturnToPool();
        }
        
    }

    void ReturnToPool()
    {
        BulletPool.Return(this);
    }


    void CollisionCheck()
    {
        var rayDir = transform.TransformDirection(moveType.moveDir);
        var rayLength = shootType.speed * Time.deltaTime;
        
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
        shootType.OnHit(other);
        
        // Apply effects
        foreach (EffectType fx in effects)
        {
            fx.OnHit(gameObject, other);
        }
        
        
        // Apply elemental effect
        if (elementType != null)
        {
            elementType.ApplyEffect(other);
        }
        
        ReturnToPool();
        
    }
    
    
    
    
// Class ends here
}

