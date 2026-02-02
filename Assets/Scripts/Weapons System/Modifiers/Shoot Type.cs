using System.Collections;
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
    
    
    [Header("Burst Stats")] 
    [Tooltip("Number of shots per burst")]
    [HideInInspector] [SerializeField] [Min(1)] private int burstAmount;
    [Tooltip("Delay between each shot in a burst")]
    [HideInInspector] [SerializeField] [Min(0.01f)] private float burstDelay;
    
    
    [Header("Multishot Stats")] 
    [HideInInspector] [Min(1)] public int bullets;
    [HideInInspector] [SerializeField] private float angle;
    [HideInInspector] public float maxAngle; // Necessary for dynamically updating slider
    [HideInInspector] public float totalSpread = 180f;
    
    
    [Header("Grapeshot Stats")] 
    [HideInInspector] [SerializeField] [Min(1)] private int pellets;
    [HideInInspector] [SerializeField] [Range(0f, 180f)] private float spread;

    public Coroutine Routine;
    

    public IEnumerator ShootRoutine(BulletPool bulletPool, Transform firePoint, Gun gun)
    {
        yield return ChosenCreate(bulletPool, firePoint, gun);
        
        // Handle fireRate
        var waitTime = 1/fireRate;
        if (fireRate == 0)
            waitTime = Mathf.Epsilon;
        yield return new WaitForSeconds(waitTime);
        Routine = null;
    }
    
    // Needs to be a coroutine os that I can yield return the BurstRoutine
    IEnumerator ChosenCreate(BulletPool bulletPool, Transform firePoint, Gun gun)
    {
        switch (behaviour)
        {
            case ShootBehaviour.Default:
                DefaultCreate(bulletPool, firePoint, gun);
                yield return null;
                break;
            
            case ShootBehaviour.Burst:
                yield return BurstCreate(bulletPool, firePoint, gun);
                break;
            
            case ShootBehaviour.Multishot:
                MultishotCreate(bulletPool, firePoint, gun);
                yield return null;
                break;
            
            case ShootBehaviour.Grapeshot:
                GrapeshotCreate(bulletPool, firePoint, gun);
                yield return null;
                break;
        }
    }

    void DefaultCreate(BulletPool bulletPool, Transform firePoint, Gun gun)
    {
        // Spawn from pool
        var newBullet = bulletPool.Spawn();
        newBullet.gun = gun;
        
        // Set position, rotation and size
        newBullet.transform.position = firePoint.position;
        newBullet.transform.rotation = firePoint.rotation;
        newBullet.transform.localScale = new Vector3(size, size, size);
        
        // Clear trailRender to prevent visual bug on bullet spawn-in
        newBullet.trail.Clear();
        
    }

    // Shoots consecutive bullets in a rapid burst
    IEnumerator BurstCreate(BulletPool bulletPool, Transform firePoint, Gun gun)
    { 
        for (int i = 0; i < burstAmount; i++)
        {
            // Spawn from pool
            var newBullet = bulletPool.Spawn();
            newBullet.gun = gun;
            
            // Set position, rotation and size
            newBullet.transform.position = firePoint.position;
            newBullet.transform.rotation = firePoint.rotation;
            newBullet.transform.localScale = new Vector3(size, size, size);

            newBullet.trail.Clear();
            
            yield return new WaitForSeconds(burstDelay);
        }
    }
    

    // Shoots multiple bullets in a fan shape
    void MultishotCreate(BulletPool bulletPool, Transform firePoint, Gun gun)
    {
        var spreadRange = (bullets - 1) * angle; // Angle between leftmost and rightmost bullet
        var startAngle = -angle / 2f;
        
        var bulletSize = new Vector3(size, size, size); // Cache bullet size
        
        for (int i = 0; i < bullets; i++)
        {
            var newBullet = bulletPool.Spawn();
            newBullet.gun = gun;
            
            // Set position, rotation and size
            newBullet.transform.position = firePoint.position;
            newBullet.transform.localScale = bulletSize;
            
            // Calculation needed to get fan spread
            var currentAngle = startAngle + (i * angle);
            var spreadRotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
            
            newBullet.transform.rotation = firePoint.rotation * spreadRotation;
            
            //var yRot = firePoint.rotation.eulerAngles.y - (spreadRange / 2) + (i * angle);
            //newBullet.transform.rotation = Quaternion.Euler(firePoint.rotation.eulerAngles.x, yRot, firePoint.rotation.eulerAngles.z);

            newBullet.trail.Clear();
        }

    }

    // Shoots multiple bullets in a 'shotgun' blast
    void GrapeshotCreate(BulletPool bulletPool, Transform firePoint, Gun gun)
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