using UnityEngine;

// Describe class function here

public class Bullet : MonoBehaviour
{   
    
    protected BaseBullet _bullet;
    
    void Start()
    {
        _bullet.SetSize(gameObject);
    }

    
    
    void Update()
    {
        Move();   
    }

    protected virtual void Move()
    {
        transform.Translate(Vector3.forward * (_bullet.Speed * Time.deltaTime));
    }


    void OnTriggerEnter(Collider other)
    {
        _bullet.OnHit(other);
        Destroy(gameObject);
    }
    
    
    
// Class ends here
}

