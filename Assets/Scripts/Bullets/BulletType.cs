using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable Objects/Bullet")]
public class BulletType : ScriptableObject
{
    
    [Header("Stats")] 
    [SerializeField] protected int damage;
    [SerializeField] protected float speed;
    [SerializeField] protected float size;
    public float lifetime;


    [Header("Visuals")] 
    [SerializeField] protected GameObject muzzleFlash;
    [SerializeField] protected TrailRenderer trail;
    [SerializeField] protected GameObject hitEffect;
    

    public void SetSize(GameObject go)
    {
        go.transform.localScale = new Vector3(size, size, size);
    }
    
    // Standard forward movement
    public virtual void Move(GameObject go)
    {
        go.transform.Translate(Vector3.forward * (speed * Time.deltaTime));
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

[CreateAssetMenu(fileName = "SideWinder", menuName = "Scriptable Objects/SideWinder")]
public class SideWinder : BulletType
{
    [Header("Oscillation")] 
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;

    private float _lifetime;

    
    private enum Orientation
    {
        Horizontal,
        Vertical
    }
    
    [SerializeField] private Orientation orientation;
    private Vector3 _direction;
    

    public override void Move(GameObject go)
    {
        // Handle orientation
        switch (orientation)
        {
            case Orientation.Horizontal:
                _direction = Vector3.right;
                break;
            case Orientation.Vertical:
                _direction = Vector3.up;
                break;
        }
        
        // Handle oscillation
        _lifetime += Time.deltaTime;
        var sin = amplitude * Mathf.Sin((2 * Mathf.PI * _lifetime * frequency) + (0.5f * Mathf.PI));
        
        // Movement
        var moveDir = (Vector3.forward + (_direction * sin)).normalized;
        go.transform.Translate( moveDir * (speed * Time.deltaTime));
    }


}

public class Spiral : BulletType
{
    
    
    
    
}