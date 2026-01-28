using System;
using Demo_Project;
using UnityEngine;

// Defines pickupType, and OnPickup behaviour

public class Pickup : MonoBehaviour
{
    
    public enum PickupType
    {
        SideWinder,
        Freeze,
        Burn,
        Explode,
        Pierce,
        Bounce
    }
    
    public PickupType type;
    
    

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (collider.TryGetComponent(out PickupHandler handler))
            {
                handler.GetPickup(type);
            }
            
            PickupSpawner.Spawned--;
            Destroy(gameObject);
        }
        
    }
    
    
    
    
// Class ends here
}
