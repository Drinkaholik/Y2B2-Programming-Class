using UnityEngine;

// Centralizes player functions & references that will be used across player components
// E.g: player weapons, the object the player is looking at

public class Player : MonoBehaviour
{
    
    // Weapons 
    public static GameObject laser; 
    
    
    
    // Look info
    public static GameObject lookObject { get; private set; }
    public static GameObject lastLookedObject { get; private set; }
    public static float lookDistance { get; private set; }
    
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }
    
    void Update()
    {
        
        
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            
            lookObject = hit.collider.gameObject; 
            lastLookedObject = lookObject;
            lookDistance = hit.distance;
            
        }
        else
        {
            lookObject = null;
            lookDistance = Mathf.Infinity;
        }
    }
    
    
    
// Class ends here
}
