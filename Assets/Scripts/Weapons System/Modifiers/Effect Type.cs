using UnityEngine;

// Holds data and methods for the bullet effects
// All effects can be joined in any combination

[CreateAssetMenu(fileName = "EffectType", menuName = "Scriptable Objects/EffectType")]
public class EffectType : ModifierType
{
    
    public enum EffectBehaviour
    {
        Pierce,
        Bounce,
        Explode
    }
    
    public EffectBehaviour behaviour;
    
    
    [Header("PierceStats")] 
    [Tooltip("Number of enemies it will pierce before despawning")]
    [Range(1, 100)] [HideInInspector] public int pierce;

    [Header("BounceStats")] 
    [Tooltip("Number of times it will bounce before despawning")]
    [Range(1, 100)] [HideInInspector] public int bounciness;



    [Header("ExplosionStats")] 
    [HideInInspector] [SerializeField] private int explosionDamage;
    [HideInInspector] [SerializeField] private float explosionRadius;
    [HideInInspector] [SerializeField] private GameObject explosionVFX;

    private Collider[] _results; // Holds results from OverlapSphere
    
    
    public void OnHit(Bullet bullet, RaycastHit hit)
    {
        switch (behaviour)
        {
            case EffectBehaviour.Pierce:
                PierceHit(bullet, hit);
                
                break;
            case EffectBehaviour.Bounce:
                BounceHit(bullet, hit);
                
                break;
            case EffectBehaviour.Explode:
                ExplodeHit(bullet, hit);
                
                break;
        }
        
        
    }

    private void PierceHit(Bullet bullet, RaycastHit hit)
    {
        if (hit.collider.CompareTag("Enemy"))
        {
            
            bullet.ignoreHit = hit.collider;
        }

    }

    private void BounceHit(Bullet bullet, RaycastHit hit)
    {
        if (hit.collider.CompareTag("Environment"))
        {
            var incomingVector = bullet.transform.forward;
            var outgoingVector = Vector3.Reflect(incomingVector, hit.normal);
            bullet.transform.forward = outgoingVector;
        }
    }

    private void ExplodeHit(Bullet bullet, RaycastHit hit)
    {
        Physics.OverlapSphereNonAlloc(bullet.transform.position, explosionRadius, _results);
        foreach (var item in _results)
        {
            // Apply damage if applicable
            if (item.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(explosionDamage);
            }
        }

        var vfxInstance = Instantiate(explosionVFX, bullet.transform.position, Quaternion.identity);
        Destroy(vfxInstance, 1f);

    }
}