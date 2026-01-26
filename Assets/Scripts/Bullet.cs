using UnityEngine;

// Describe class function here

public class Bullet : MonoBehaviour
{
    
    [SerializeField] private BulletData _bulletData;
    

    
    void Start()
    {
        transform.localScale = new Vector3(_bulletData.Size,  _bulletData.Size, _bulletData.Size);
    }

    
    
    void Update()
    {
        transform.Translate(Vector3.forward * (_bulletData.Speed * Time.deltaTime));
    }


    void OnTriggerEnter(Collider other)
    {
        // Apply damage if applicable
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_bulletData.Damage);
        }
        
        Destroy(gameObject);
    }
    
    
    
// Class ends here
}
