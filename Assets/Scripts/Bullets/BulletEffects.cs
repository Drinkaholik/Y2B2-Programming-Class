using UnityEngine;

// Describe class function here

public abstract class BulletEffect : ScriptableObject
{
    
    public abstract void ApplyEffect(GameObject bullet, Collider other);
    
}



[CreateAssetMenu(fileName = "FreezeBullet", menuName = "Scriptable Objects/FreezeBullet")]
public class FreezeBullet : BulletEffect
{
    
    [SerializeField] private int freezeAmount;
    
    public override void ApplyEffect(GameObject bullet, Collider other)
    {
        // Apply freeze if applicable
        if (other.TryGetComponent(out IFreezable freezable))
            freezable.Freeze(freezeAmount);
        
    }
}


[CreateAssetMenu(fileName = "BurnBullet", menuName = "Scriptable Objects/BurnBullet")]
public class BurnBullet : BulletEffect
{

    [SerializeField] private float burnDuration;
    
    public override void ApplyEffect(GameObject bullet, Collider other)
    {
        // Apply burn if applicable
        if (other.TryGetComponent(out IBurnable burnable))
        {
            burnable.Burn(burnDuration);
        }
    }
    
}

[CreateAssetMenu(fileName = "ExplodeBullet", menuName = "Scriptable Objects/ExplodeBullet")]
public class ExplodeBullet : BulletEffect
{
    
    [SerializeField] private int explosionDamage;
    [SerializeField] private float radius;
    [SerializeField] private GameObject vfx;
    
    public override void ApplyEffect(GameObject bullet, Collider other)
    {
        var sphere = Physics.OverlapSphere(bullet.transform.position, radius);
        foreach (var item in sphere)
        {
            // Apply damage if applicable
            if (item.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(explosionDamage);
            }
        }
        
        var temp = Instantiate(vfx, bullet.transform.position, Quaternion.identity);
        Destroy(temp, 1f);
       
    }
    
}

