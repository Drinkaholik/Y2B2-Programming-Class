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
    // Needed to assign bullet modifiers in Gun - might not need it after doing object pooling
    [HideInInspector] public GameObject newBullet;


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
   

    public IEnumerator ShootRoutine(GameObject bullet, Transform firePoint, float timer)
    {
        ChosenCreate(bullet, firePoint, timer);
        
        // Handle fireRate
        var waitTime = 1/fireRate;
        if (fireRate == 0)
            waitTime = Mathf.Epsilon;
        yield return new WaitForSeconds(waitTime);
        Routine = null;
    }
    
    void ChosenCreate(GameObject bullet, Transform firePoint, float timer)
    {
        switch (behaviour)
        {
            case ShootBehaviour.Default:
                DefaultCreate(bullet, firePoint);
                break;
            
            case ShootBehaviour.Burst:
                
                BurstCreate(bullet, firePoint, timer);
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
        newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        newBullet.transform.localScale = new Vector3(size, size, size);
    }

    // Doesn't work yet because newBullet is changed 3 times before the gun gains access to it
    void BurstCreate(GameObject bullet, Transform firePoint, float timer)
    {
        for (int i = 0; i < burstAmount; i++)
        {
            timer += Time.deltaTime;
            if (timer >= burstDelay)
            {
                newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
                newBullet.transform.localScale = new Vector3(size, size, size);
                timer = 0;
            }
        }
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