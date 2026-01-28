using System;
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
            // PickupHandler is placed on player
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
