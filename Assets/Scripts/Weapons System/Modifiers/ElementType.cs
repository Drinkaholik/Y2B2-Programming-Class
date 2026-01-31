using UnityEngine;

// Holds info and methods for the bullet elements
// All elements are mutually exclusive
// Base is abstract

[CreateAssetMenu(fileName = "ElementType", menuName = "Scriptable Objects/ElementType")]
public abstract class ElementType : ScriptableObject
{
    public abstract void ApplyEffect(GameObject bullet, Collider other);
}


[CreateAssetMenu(fileName = "FreezeBullet", menuName = "Scriptable Objects/FreezeBullet")]
public class FreezeBullet : ElementType
{
    
    [SerializeField] private int freezeAmount;
    
    public override void ApplyEffect(GameObject bullet, Collider other)
    {
        // Apply freeze if applicable
        if (other.TryGetComponent(out IFreezable freezable))
            freezable.Freeze(freezeAmount);
        
    }
}


[CreateAssetMenu(fileName = "BurnBullet", menuName = "Scriptable Objects/BurnBullet")]
public class BurnBullet : ElementType
{

    [SerializeField] private float burnDuration;
    
    public override void ApplyEffect(GameObject bullet, Collider other)
    {
        // Apply burn if applicable
        if (other.TryGetComponent(out IBurnable burnable))
        {
            burnable.Burn(burnDuration);
        }
    }
    
}
