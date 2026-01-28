using UnityEngine;

// Parses pickups and sends the necessary info to the bullet script

public class PickupHandler : MonoBehaviour
{
    
    public BulletType sideWinder;
    public BulletEffect freeze;
    public BulletEffect burn;
    public BulletEffect explode;
    public BulletEffect pierce;
    public BulletEffect bounce;
    
    public void GetPickup(Pickup.PickupType type)
    {
        switch (type)
        {
            case Pickup.PickupType.SideWinder:
            
                
                break;
            case Pickup.PickupType.Freeze:
            
                
                break;
            case Pickup.PickupType.Burn:

                
                break;
            case Pickup.PickupType.Explode:
            
                
                break;
            case Pickup.PickupType.Pierce:
            
                
                break;
            case Pickup.PickupType.Bounce:
            
                
                break;
            
        }
            

    }

    
    
    
    
// Class ends here
}
