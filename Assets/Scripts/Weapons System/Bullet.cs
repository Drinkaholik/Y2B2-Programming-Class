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
    
    // Needed for effects
    private bool _despawn; // Only despawn on hit if true
    public Collider ignoreHit; // Needed so pierced enemies arent hit every frame
    public int pierceCount;
    public int bounceCount;

    private Renderer _rend;
    
    // Why do I need a getter here? simply setting it in start doesnt work....
    public Renderer rend
    {
        get
        {
            if  (_rend == null) _rend = GetComponent<Renderer>();
            return _rend;
        }
    }
    

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        //rend = GetComponent<Renderer>();
    }

    void Awake()
    {
        _despawn = false;
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
        var rayLength = (shootType.speed * Time.deltaTime) + 0.1f; // Need buffer to prevent tunnelling
        
        //Debug.DrawRay(transform.position, rayDir * (rayLength * 10), Color.red, 1f);
        
        if (Physics.Raycast(transform.position, rayDir, out RaycastHit hit, rayLength))
        {
            transform.position = hit.point;
            
            OnHit(hit);
        }
        
    }
    
    
    // On hit
    void OnHit(RaycastHit hit)
    {
        if (hit.collider != ignoreHit)
        {
            // Apply damage
            shootType.OnHit(hit.collider);
        
            // Apply effects
            foreach (EffectType fx in effects)
            {
                fx.OnHit(this, hit);
            }
            
            // Apply elemental effect
            if (elementType != null)
            {
                elementType.ApplyEffect(hit.collider);
            }
            
            ignoreHit = null;
            
            if (_despawn)
                ReturnToPool();

            
        }
        
    }
    
    
    
    
// Class ends here
}

