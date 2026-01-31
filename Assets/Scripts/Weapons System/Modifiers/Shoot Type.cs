using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds info and methods for gun/bullet shooting behaviour
// All shoot types are mutually exclusive

[CreateAssetMenu(fileName = "ShootType", menuName = "Scriptable Objects/ShootType")]
public class ShootType : ScriptableObject
{
    
    public enum ShootBehaviour
    {
        Default,
        Burst,
        Multishot,
        Grapeshot
    }

    public ShootBehaviour behaviour;
    
    [Header("General Stats")] 
    [Min(0)] [SerializeField] private int damage;
    [Min(0)] public float fireRate;
    [Min(0)] public float speed;
    
    [Min(0.01f)] [SerializeField] private float size;
    public float lifetime;
    // Needed to assign bullet modifiers in Gun - might not need it after doing object pooling
    [HideInInspector] public List<Bullet> spawnedBullets = new (100);


    [Header("Burst Stats")] 
    [Tooltip("Number of shots per burst")]
    [HideInInspector] [SerializeField] [Min(1)] private int burstAmount;
    [Tooltip("Delay between each shot in a burst")]
    [HideInInspector] [SerializeField] [Min(0.01f)] private float burstDelay;
    
    
    [Header("Multishot Stats")] 
    [HideInInspector] [Min(1)] public int bullets;
    [HideInInspector] [SerializeField] private float angle;
    // Necessary for a dynamically updated max angle
    [HideInInspector] public float maxAngle;
    [HideInInspector] public float totalSpread = 180f;
    
    [Header("Grapeshot Stats")] 
    [HideInInspector] [SerializeField] [Min(1)] private int pellets;
    [HideInInspector] [SerializeField] [Range(0f, 180f)] private float spread;

    public Coroutine Routine;
   

    public IEnumerator ShootRoutine(BulletPool bulletPool, Transform firePoint, float timer)
    {
        ChosenCreate(bulletPool, firePoint, timer);
        
        // Handle fireRate
        var waitTime = 1/fireRate;
        if (fireRate == 0)
            waitTime = Mathf.Epsilon;
        yield return new WaitForSeconds(waitTime);
        Routine = null;
    }
    
    void ChosenCreate(BulletPool bulletPool, Transform firePoint, float timer)
    {
        switch (behaviour)
        {
            case ShootBehaviour.Default:
                DefaultCreate(bulletPool, firePoint);
                break;
            
            case ShootBehaviour.Burst:
                
                BurstCreate(bulletPool, firePoint, timer);
                break;
            
            case ShootBehaviour.Multishot:
                MultishotCreate(bulletPool, firePoint);
                break;
            
            case ShootBehaviour.Grapeshot:
                GrapeshotCreate(bulletPool, firePoint);
                break;
        }
    }

    void DefaultCreate(BulletPool bulletPool, Transform firePoint)
    {
        // Clear list - necessary for setting each bullet's gun reference in the Gun class
        spawnedBullets.Clear();
        
        // Spawn from pool
        var newBullet = bulletPool.Spawn();
        spawnedBullets.Add(newBullet);
        
        // Set position, rotation and size
        newBullet.transform.position = firePoint.position;
        newBullet.transform.rotation = firePoint.rotation;
        newBullet.transform.localScale = new Vector3(size, size, size);
        
    }

    // Shoots consecutive bullets in a rapid burst
    void BurstCreate(BulletPool bulletPool, Transform firePoint, float timer)
    {
        spawnedBullets.Clear();
        for (int i = 0; i < burstAmount; i++)
        {
            timer += Time.deltaTime;
            if (timer >= burstDelay)
            {
                // Spawn and add to list
                var newBullet = bulletPool.Spawn();
                spawnedBullets.Add(newBullet);
                
                // Set position, rotation and size
                newBullet.transform.position = firePoint.position;
                newBullet.transform.rotation = firePoint.rotation;
                newBullet.transform.localScale = new Vector3(size, size, size);
                
                // Reset timer
                timer = 0;
            }
        }
    }

    // Shoots multiple bullets in a fan shape
    void MultishotCreate(BulletPool bulletPool, Transform firePoint)
    {
        spawnedBullets.Clear();

    }

    // Shoots multiple bullets in a 'shotgun' blast
    void GrapeshotCreate(BulletPool bulletPool, Transform firePoint)
    {
        spawnedBullets.Clear();
        
        
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