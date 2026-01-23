using UnityEngine;

// Push rigidbodies around with a given force

public class Push : MonoBehaviour
{

    [SerializeField] private float pushForce;
    [SerializeField] private Transform startPos;
    [SerializeField] private ForceMode forceMode;
    
    private GameObject pushObject;
    private Rigidbody cachedRB;
    
    // Using this interface means I don't need a direct reference to the Charge script
    // Instead, I can just check if any script attached to the same gameObject has the charge interface
    private IChargeInterface iCharge; 


    private void Start()
    {
        // Checks ONLY ON SAME GAMEOBJECT for script interface
        iCharge = GetComponent<IChargeInterface>();
        
    }
    
    public void PushObject(GameObject obj)
    {

        // Cache pushObject to save on GetComponent calls
        if (obj != pushObject)
        {
            // Target is new, so set rigidbody
            if (obj.TryGetComponent(out Rigidbody newRb)) 
            {
                cachedRB = newRb;
                pushObject = obj;
            }
            
            else //target has no rigidbody, clear cache and exit
            {
                cachedRB = null;
                pushObject = null;
                return;
            }
        }
        
        // If obj has non-kinematic rb, then apply force
        if (cachedRB != null && !cachedRB.isKinematic)
        {
            
            // Vary pushForce based on charge level
            float multiplier = 1;

            if (iCharge != null)
            {
                multiplier = iCharge.GetCharge();
            }
            
            var finalForce = pushForce * multiplier;
            
            var pushDir = Vector3.Normalize(obj.transform.position - startPos.position);
            cachedRB.AddForce(pushDir * finalForce, forceMode);
        }
    }
    
// Class ends here
}
