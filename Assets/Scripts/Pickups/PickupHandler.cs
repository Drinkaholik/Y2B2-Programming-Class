using System.Collections.Generic;
using UnityEngine;

// Parses pickups and sends the necessary info to the currently equipped gun

public class PickupHandler : MonoBehaviour
{
    
    // Loops through list of effects on hit, calling ApplyEffect for each one. 
    public static HashSet<EffectType> BulletEffects;
    
    // Defines bullet movement type
    public static MovementType MovementType;

    public static ElementType ElementType;

    public static ShootType ShootType;
    
    
    
    public void GetPickup(Pickup.PickupType type)
    {
        

    }

    
    
    
    
// Class ends here
}
