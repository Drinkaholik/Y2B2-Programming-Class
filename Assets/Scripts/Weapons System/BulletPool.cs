using System.Collections.Generic;
using UnityEngine;

// Builds a bullet pool to be used by player weapons

public class BulletPool : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    
    [SerializeField] private int poolSize;
    
    public Queue <Bullet> bulletPool;
    
    void Start()
    {
        InstantiateToPool(poolSize);
    }

    void InstantiateToPool(int count)
    {
        // Add bullets to pool
        for (int i = 0; i < count; i++)
        {
            var bulletComponent = Instantiate(bulletPrefab).GetComponent<Bullet>();
            bulletComponent.gameObject.SetActive(false);
        
            bulletPool.Enqueue(bulletComponent);
        }
        
        
    }

    public Bullet Spawn()
    {
        // Add more bullets to pool if you run out - just in case
        if (bulletPool.Count == 0) InstantiateToPool(1);
        
        var bullet = bulletPool.Dequeue();
        bullet.gameObject.SetActive(true);
        return bullet;
        
    }

    public void Return(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bulletPool.Enqueue(bullet);
        
    }
    
    
// Class ends here
}
