using UnityEngine;

// Holds info and methods for bullet shooting behaviour
// All shoot types are mutually exclusive
// Base is instantiable

[CreateAssetMenu(fileName = "ShootType", menuName = "Scriptable Objects/ShootType")]
public class ShootType : ScriptableObject
{

    [Header("Stats")] 
    [SerializeField] protected int damage;
    public float fireRate;
    public float speed;
    
    [SerializeField] protected float size;
    public float lifetime;


    public virtual void Shoot(GameObject bullet, Transform firePoint)
    {
        bullet.transform.localScale = new Vector3(size, size, size);
        Instantiate(firePoint, firePoint.position, firePoint.rotation);
    }

    public void OnHit(Collider other)
    {
        // Apply damage if applicable
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }
    }
    

}


[CreateAssetMenu(fileName = "TripleShot", menuName = "Scriptable Objects/TripleShot")]
public class TripleShot : ShootType
{
    public override void Shoot(GameObject bullet, Transform firePoint)
    {
        
    }
}


[CreateAssetMenu(fileName = "Grapeshot", menuName = "Scriptable Objects/Grapeshot")]
public class Grapeshot : ShootType
{
    
    public override void Shoot(GameObject bullet, Transform firePoint)
    {
        
    }
    
}