using System.Collections.Generic;
using UnityEngine;

// Builds a bullet pool to be used by player weapons

public class BulletPool : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    
    [SerializeField] private int poolSize;

    private Queue<Bullet> _bulletPool = new();
    
    void Start()
    {
        InstantiateToPool(poolSize);
    }

    void InstantiateToPool(int count)
    {
        // Add bullets to pool
        for (int i = 0; i < count; i++)
        {
            var bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
            
            _bulletPool.Enqueue(bullet);
            bullet.transform.SetParent(transform); // Keeps the hierarchy cleaner - all bullets are under the controller
            bullet.gameObject.SetActive(false);
        }
    }

    public Bullet Spawn()
    {
        // Add more bullets to pool if you run out - just in case
        if (_bulletPool.Count == 0) InstantiateToPool(1);
        
        var bullet = _bulletPool.Dequeue();
        bullet.gameObject.SetActive(true);
        
        return bullet;
        
    }

    public void Return(Bullet bullet)
    {
        _bulletPool.Enqueue(bullet);
        bullet.gameObject.SetActive(false);
    }
    
    
// Class ends here
}
