using UnityEngine;

// Defines pickupType, and OnPickup behaviour

public class Pickup : MonoBehaviour
{
    
    
    public ModifierType modifier;
    
    

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // PickupHandler is placed on player
            if (collider.TryGetComponent(out PickupHandler handler))
            {
                handler.GetPickup(modifier);
            }
            
            Destroy(gameObject);
        }
    }
    
    
    
    
// Class ends here
}
