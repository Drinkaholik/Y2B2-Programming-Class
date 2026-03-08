using UnityEngine;

// Parses pickups and sends the necessary info to the currently equipped gun

public class PickupHandler : MonoBehaviour
{
    
    public Gun equippedGun;
    
    public void GetPickup(ModifierType modifier)
    {
        switch (modifier)
        {
            case (ShootType s):
                equippedGun.shootType = s;
                break;
            
            case (MoveType m):
                equippedGun.moveType = m;
                break;
            
            case EffectType ef:
                equippedGun.effects.Add(ef);
                break;
            
            case ElementType el:
                equippedGun.elementType = el;
                break;
        }
    }
    
// Class ends here
}
