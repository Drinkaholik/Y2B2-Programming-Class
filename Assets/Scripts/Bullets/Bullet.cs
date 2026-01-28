using System.Collections.Generic;
using UnityEngine;

// Template for BulletType and BulletEffect to apply their modifiers onto

public class Bullet : MonoBehaviour
{
    
    // Loops through list of effects on hit, calling ApplyEffect for each one. 
    public List<BulletEffect> _bulletEffects;
    
    // Defines bullet movement type
    public BulletType _bulletType;

    private float _timeLived;

    private enum Element
    {
        None,
        Burn,
        Freeze
    }
    
   
    
    void Start()
    {
        _bulletType.SetSize(gameObject);
    }

    
    
    void Update()
    {
        CheckLifetime();
        _bulletType.Move(gameObject);
    }

    void CheckLifetime()
    {
        _timeLived += Time.deltaTime;

        if (_timeLived >= _bulletType.lifetime)
        {
            Destroy(gameObject);
        }
        
    }

    // On hit
    void OnTriggerEnter(Collider other)
    {
        // Apply damage
        _bulletType.OnHit(other);
        
        foreach (BulletEffect fx in _bulletEffects)
        {
            fx.ApplyEffect(gameObject, other);
        }
        
        Destroy(gameObject);
    }
    
    
    
// Class ends here
}

