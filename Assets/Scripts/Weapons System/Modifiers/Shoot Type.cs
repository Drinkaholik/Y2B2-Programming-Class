using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds data and methods for gun/bullet shooting behaviour
// All shoot types are mutually exclusive
// Uses a switch statement instead of inheritance for designer friendliness

[CreateAssetMenu(fileName = "ShootType", menuName = "Scriptable Objects/ShootType")]
public class ShootType : ModifierType
{
    
    public enum ShootBehaviour
    {
        Default,
        Burst,
        Spreadshot,
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
    
    
    // Spreadshot stats
    [Tooltip("Angle between each bullet in the spread")]
    [HideInInspector] [SerializeField] private float angle;
    [HideInInspector] public float maxAngle; // Necessary for dynamically updating slider
    [HideInInspector] public float totalSpread = 180f;

    public enum SpreadOrientation
    {
        Horizontal,
        Vertical
    }
    
    [Header("Multishot Stats")] // Idk why but putting the header here makes it display correctly in inspector
    // Grapeshot stats
    [HideInInspector] [Min(1)] public int bullets;
    [HideInInspector] [SerializeField] [Range(0f, 180f)] private float spread;
    [HideInInspector] [SerializeField] private SpreadOrientation spreadOrientation;
    
    public Coroutine Routine;
    

    public IEnumerator ShootRoutine(Transform firePoint, ShootType shootType, MoveType moveType, List<EffectType> effects, ElementType elementType)
    {
        yield return ChosenCreate(firePoint, shootType, moveType, effects, elementType);
        
        // Handle fireRate
        var waitTime = 1/fireRate;
        if (fireRate == 0)
            waitTime = Mathf.Epsilon;
        yield return new WaitForSeconds(waitTime);
        Routine = null;
    }
    
    // Needs to be a coroutine so that I can yield return the BurstRoutine
    IEnumerator ChosenCreate(Transform firePoint, ShootType shootType, MoveType moveType, List<EffectType> effects, ElementType elementType)
    {
        switch (behaviour)
        {
            case ShootBehaviour.Default:
                DefaultCreate(firePoint, shootType, moveType, effects, elementType);
                yield return null;
                break;
            
            case ShootBehaviour.Burst:
                yield return BurstCreate(firePoint, shootType, moveType, effects, elementType);
                break;
            
            case ShootBehaviour.Spreadshot:
                SpreadshotCreate(firePoint, shootType, moveType, effects, elementType);
                yield return null;
                break;
            
            case ShootBehaviour.Grapeshot:
                GrapeshotCreate(firePoint, shootType, moveType, effects, elementType);
                yield return null;
                break;
        }
    }

    void DefaultCreate(Transform firePoint, ShootType shootType, MoveType moveType, List<EffectType> effects, ElementType elementType)
    {
        // Spawn from pool
        var newBullet = BulletPool.Spawn();
        
        SetModifiers(newBullet, shootType, moveType, effects, elementType);
        SetMaterials(newBullet, elementType);
        SetTransform(newBullet, firePoint);
        
        // Clear trailRender to prevent visual bug on bullet spawn-in
        newBullet.trail.Clear();
    }

    // Shoots consecutive bullets in a rapid burst
    IEnumerator BurstCreate(Transform firePoint, ShootType shootType, MoveType moveType, List<EffectType> effects, ElementType elementType)
    { 
        for (int i = 0; i < burstAmount; i++)
        {
            // Spawn from pool
            var newBullet = BulletPool.Spawn();
            
            SetModifiers(newBullet, shootType, moveType, effects, elementType);
            SetMaterials(newBullet, elementType);
            SetTransform(newBullet, firePoint);

            newBullet.trail.Clear();
            
            yield return new WaitForSeconds(burstDelay);
        }
    }
    

    // Shoots multiple bullets in a fan shape
    void SpreadshotCreate(Transform firePoint, ShootType shootType, MoveType moveType, List<EffectType> effects, ElementType elementType)
    {
        var startAngle = (-angle * bullets) / 2f;
        
        var bulletSize = new Vector3(size, size, size); // Cache bullet size
        
        for (int i = 0; i < bullets; i++)
        {
            var newBullet = BulletPool.Spawn();
            
            SetModifiers(newBullet, shootType, moveType, effects, elementType);
            SetMaterials(newBullet, elementType);
            
            // Set position and size
            newBullet.transform.position = firePoint.position;
            newBullet.transform.localScale = bulletSize;
            
            // Calculation needed to get fan spread
            var currentAngle = startAngle + (i * angle);
            var spreadRotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
            if (spreadOrientation == SpreadOrientation.Vertical)
            {
                spreadRotation = Quaternion.AngleAxis(currentAngle, Vector3.right);
            }
            
            newBullet.transform.rotation = firePoint.rotation * spreadRotation;
            
            newBullet.trail.Clear();
        }

    }

    // Shoots multiple bullets in a 'shotgun' blast
    void GrapeshotCreate(Transform firePoint, ShootType shootType, MoveType moveType, List<EffectType> effects, ElementType elementType)
    {
        var bulletSize = new Vector3(size, size, size); // Cache bullet size
        
        for (int i = 0; i < bullets; i++)
        {
            var newBullet = BulletPool.Spawn();
            
            SetModifiers(newBullet, shootType, moveType, effects, elementType);
            SetMaterials(newBullet, elementType);
            
            // Set position and size
            newBullet.transform.position = firePoint.position;
            newBullet.transform.localScale = bulletSize;
            
            // Calculation needed to get spread - I hate working with rotations
            var pelletSpread = Random.insideUnitCircle * spread;
            var xSpread = Quaternion.AngleAxis(pelletSpread.x, Vector3.up);
            var ySpread = Quaternion.AngleAxis(pelletSpread.y, Vector3.right);
            
            newBullet.transform.rotation = firePoint.rotation * xSpread * ySpread;
            
            newBullet.trail.Clear();
        }
    }

    
    
    public void OnHit(Collider other)
    {
        // Apply damage if applicable
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }
    }

    
    // Set bullet properties //

    private void SetModifiers(Bullet bullet, ShootType shootType, MoveType moveType, List<EffectType> effects, ElementType elementType)
    {
        bullet.shootType = shootType;
        bullet.moveType = moveType;
        bullet.effects = effects;
        bullet.elementType = elementType;
    }
    
    private void SetMaterials(Bullet bullet, ElementType elementType)
    {
        bullet.rend.sharedMaterial = elementType.material;
        bullet.trail.sharedMaterial = elementType.trailMaterial;
    }

    void SetTransform(Bullet bullet, Transform firePoint)
    {
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.transform.localScale = new Vector3(size, size, size);
    }
    

}