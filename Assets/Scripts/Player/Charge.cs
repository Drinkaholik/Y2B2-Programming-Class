using UnityEngine;

// Relates to varying a "charge" number between 0 and 1, used to scale other variables.

public interface IChargeInterface
{

    float charge { get; }
    float GetCharge();
    
}

public class Charge : MonoBehaviour, IChargeInterface
{

    [SerializeField] [Tooltip("Seconds taken to charge to max")] private float chargeTime;
    [SerializeField] [Tooltip("Seconds taken to discharge completely")] private float dischargeTime;
    
    // Necessary for IChargeInterface
    public float charge { get; private set; } 
    
    public float GetCharge()
    {
        return charge;
    }
    
   
    
    
    public void Charging()
    {
        charge += (1 / chargeTime * Time.deltaTime);
        
        charge = Mathf.Clamp(charge, 0f, 1f);
    }

    public void Discharging()
    {
        charge -= (1 / dischargeTime * Time.deltaTime);
        
        charge = Mathf.Clamp(charge, 0f, 1f);
    }

    public void ResetCharge()
    {
        charge = 0f;
    }

    public void MaxCharge()
    {
        charge = 1;
    }
    
    
// Class ends here
}
