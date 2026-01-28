using System.Collections;
using UnityEngine;

// Holds info and methods for bullet shooting behaviour
// All shoot types are mutually exclusive
// Base is instantiable

[CreateAssetMenu(fileName = "ShootType", menuName = "Scriptable Objects/ShootType")]
public class ShootType : ScriptableObject
{

    [Header("General Stats")] 
    [SerializeField] private int damage;
    public float fireRate;
    public float speed;
    
    [SerializeField] private float size;
    public float lifetime;


    [Header("Burst Stats")] 
    [SerializeField] private int burstAmount;
    [SerializeField] private float delay;
    
    
    [Header("Multishot Stats")] 
    [SerializeField] private float bullets;
    [SerializeField] private float angle;
    
    
    
    [Header("Grapeshot Stats")] 
    [SerializeField] private float pellets;
    [SerializeField] private float spread;
    
   

    public enum ShootBehaviour
    {
        Default,
        Burst,
        Multishot,
        Grapeshot
    }

    public ShootBehaviour shootBehaviour;


    public virtual IEnumerator Shoot(GameObject bullet, Transform firePoint)
    {
        ChosenCreate(bullet, firePoint);
        
        // Handle fireRate
        var waitTime = 1/fireRate;
        if (fireRate == 0)
            waitTime = Mathf.Epsilon;
        
        yield return new WaitForSeconds(waitTime);
    }
    
    void ChosenCreate(GameObject bullet, Transform firePoint)
    {
        switch (shootBehaviour)
        {
            case ShootBehaviour.Default:
                DefaultCreate(bullet, firePoint);
                break;
            
            case ShootBehaviour.Burst:
                BurstCreate(bullet, firePoint);
                break;
            
            case ShootBehaviour.Multishot:
                MultishotCreate(bullet, firePoint);
                break;
            
            case ShootBehaviour.Grapeshot:
                GrapeshotCreate(bullet, firePoint);
                break;
        }
    }

    void DefaultCreate(GameObject bullet, Transform firePoint)
    {
        // Instantiate at correct size
        var b = Instantiate(bullet, firePoint.position, firePoint.rotation);
        b.transform.localScale = new Vector3(size, size, size);
    }

    void BurstCreate(GameObject bullet, Transform firePoint)
    {
        
        
    }


    void MultishotCreate(GameObject bullet, Transform firePoint)
    {
        

    }


    void GrapeshotCreate(GameObject bullet, Transform firePoint)
    {
        
        
        
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