using System.Collections;
using Sirenix.OdinInspector;
using SQLite;
using UnityEngine;

// Holds info and methods for bullet shooting behaviour
// All shoot types are mutually exclusive
// Base is instantiable

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

    public ShootBehaviour shootBehaviour;
    
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
    // Necessary for a dynamically updated max angle
    [HideInInspector] public float maxAngle;
    [HideInInspector] public float totalSpread = 180f;
    
    [Header("Grapeshot Stats")] 
    [HideInInspector] [SerializeField] [Min(1)] private int pellets;
    [HideInInspector] [SerializeField] [Range(0f, 180f)] private float spread;

    public Coroutine Routine;

    public IEnumerator ShootRoutine(GameObject bullet, Transform firePoint)
    {
        ChosenCreate(bullet, firePoint);
        
        // Handle fireRate
        var waitTime = 1/fireRate;
        if (fireRate == 0)
            waitTime = Mathf.Epsilon;
        yield return new WaitForSeconds(waitTime);
        Routine = null;
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
        var newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        newBullet.transform.localScale = new Vector3(size, size, size);
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

    // Destroy if in scene for too long
    public void CheckLifetime(GameObject bullet, float timeElapsed)
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= lifetime)
        {
            Destroy(bullet);
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
    

}