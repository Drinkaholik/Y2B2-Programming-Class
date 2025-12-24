using UnityEngine;

// Describe class function here

public class Spin : MonoBehaviour
{
    [SerializeField] private GameObject spinObject;
    [SerializeField] [Tooltip("RPM")] private float  spinTime;
    [SerializeField] [Tooltip("Axis to spin around")] private Vector3 spinAxis;

    [SerializeField] [Tooltip("Whether the object takes time to spin up / down")] private bool spinUp;
    [SerializeField] [Tooltip("Time taken for spinTime to increase by 1")] private float spinAccel;
    [SerializeField] [Tooltip("Time taken for spinTime to decrease by 1")] private float spinDecel;
    private float realSpin;
    
    public void SpinObject()
    {
        // Start spinning at max speed if spinUp = false
        if (!spinUp)
        {
            realSpin = spinTime;
        }
        
        spinObject.transform.Rotate(spinAxis, 360 * realSpin * Time.deltaTime);
        
    }

    public void SpinUp()
    {
        // Only spin up if below spinTime
        
        if (realSpin >= spinTime) return; 
        
        realSpin += spinAccel * Time.deltaTime;
        
        realSpin = Mathf.Clamp(realSpin, 0, spinTime);
    }
    
    public void SpinDown()
    {
        // Only spin down if above 0
        
        if (realSpin <= 0) return; 
       
        realSpin -= spinDecel * Time.deltaTime;
        
        realSpin = Mathf.Clamp(realSpin, 0, spinTime);
    }
    
    
    
// Class ends here
}
