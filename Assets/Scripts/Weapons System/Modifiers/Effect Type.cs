using UnityEngine;

// Holds info and methods for the bullet effects
// All effects can be joined in any combination
// Base is abstract

[CreateAssetMenu(fileName = "EffectType", menuName = "Scriptable Objects/EffectType")]
public class EffectType : ScriptableObject
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
    public int pierce;

    [Header("BounceStats")] 
    [Tooltip("Number of times it will bounce before despawning")]
    public int bounciness;



    [Header("ExplosionStats")] [SerializeField]
    private int explosionDamage;
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject explosionVFX;

    private Collider[] _results; // Holds results from OverlapSphere


    public void OnHit(GameObject bullet, Collider other)
    {
        switch (behaviour)
        {
            case EffectBehaviour.Pierce:
                PierceHit(bullet, other);
                
                break;
            case EffectBehaviour.Bounce:
                BounceHit(bullet, other);
                
                break;
            case EffectBehaviour.Explode:
                ExplodeHit(bullet, other);
                
                break;
        }
        
        
    }

    private void PierceHit(GameObject bullet, Collider other)
    {


    }

    private void BounceHit(GameObject bullet, Collider other)
    {

    }

    private void ExplodeHit(GameObject bullet, Collider other)
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