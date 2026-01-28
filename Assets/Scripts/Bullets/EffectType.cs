using UnityEngine;

// Holds info and methods for the bullet effects
// All effects can be joined in any combination
// Base is abstract

[CreateAssetMenu(fileName = "EffectType", menuName = "Scriptable Objects/EffectType")]
public abstract class EffectType : ScriptableObject
{
    public abstract void OnHit(GameObject bullet, Collider other);
}


[CreateAssetMenu(fileName = "ExplodeBullet", menuName = "Scriptable Objects/ExplodeBullet")]
public class ExplodeBullet : EffectType
{
    
    [SerializeField] private int explosionDamage;
    [SerializeField] private float radius;
    [SerializeField] private GameObject vfx;

    private Collider[] _results; // Holds results from OverlapSphere
    
    public override void OnHit(GameObject bullet, Collider other)
    {
        Physics.OverlapSphereNonAlloc(bullet.transform.position, radius, _results);
        foreach (var item in _results)
        {
            // Apply damage if applicable
            if (item.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(explosionDamage);
            }
        }
        
        var vfxInstance = Instantiate(vfx, bullet.transform.position, Quaternion.identity);
        Destroy(vfxInstance, 1f);
       
    }
    
}