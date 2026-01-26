using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable Objects/Bullet")]
public class BaseBullet : ScriptableObject
{
    [Header("Stats")]
    [SerializeField] protected int damage;
    [SerializeField] protected float speed;
    [SerializeField] protected float size;
    [SerializeField] protected int pierce;
    
    [Header("Visuals")]
    [SerializeField] protected GameObject muzzleFlash;
    [SerializeField] protected TrailRenderer trail;
    [SerializeField] protected GameObject hitEffect;
    
    
    public float Speed => speed;
    public TrailRenderer Trail => trail;

    public void SetSize(GameObject go)
    {
        go.transform.localScale = new Vector3(size, size, size);
    }
    

    public virtual void OnHit(Collider other)
    {
        DealDamage(other);
    }

    protected void DealDamage(Collider other)
    {
        // Apply damage if applicable
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }
        
    }
}

[CreateAssetMenu(fileName = "FreezeBullet", menuName = "Scriptable Objects/FreezeBullet")]
public class FreezeBullet : BaseBullet
{

    [SerializeField] private int freezeAmount;
    
    public override void OnHit(Collider other)
    {
        DealDamage(other);
        Freeze(other);
        
    }

    private void Freeze(Collider other)
    {
        // Apply damage if applicable
        if (other.TryGetComponent(out IFreezable freezable))
            freezable.Freeze(freezeAmount);
        
    }
}


[CreateAssetMenu(fileName = "BurnBullet", menuName = "Scriptable Objects/BurnBullet")]
public class BurnBullet : BaseBullet
{

    [SerializeField] private float burnDuration;
    
    public override void OnHit(Collider other)
    {
        DealDamage(other);
        Burn(other);
        
    }

    private void Burn(Collider other)
    {
        // Apply damage if applicable
        if (other.TryGetComponent(out IBurnable burnable))
        {
            burnable.Burn(burnDuration);
        }
    }
    
}
