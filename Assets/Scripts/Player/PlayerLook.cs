using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

// Centralizes player functions & references that will be used across player components
// E.g: player weapons, the object the player is looking at

public class PlayerLook : MonoBehaviour
{
    
    [Header("Looking")]
    [SerializeField] private float mouseSensitivity;
    private float xRotation;
    private float yRotation;
    
    private InputAction _lookAction;
    
    // Look info
    public static GameObject lookObject { get; private set; }
    public static GameObject lastLookedObject { get; private set; }
    public static float lookDistance { get; private set; }
    
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        
        _lookAction = InputSystem.actions.FindAction("Look");
    }
    
    void Update()
    {
        
        MouseLook();
        LookInfo();
        

    }
    
    private void MouseLook() // Mouse movement
    {
        
        var lookVector = _lookAction.ReadValue<Vector2>(); // Get mouse movement input
    
        // Look up and down - only moves camera, not player object
        float mouseY = lookVector.y * mouseSensitivity * Time.deltaTime; // Multiply by sensitivity and deltaTime
    
        xRotation -= mouseY; // Control rotation around x-axis
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the rotation so you can't flip camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Apply to camera transform
    
        // Look left and right - moves player object
        float mouseX = lookVector.x * mouseSensitivity * Time.deltaTime;
    
        yRotation += mouseX; // Control rotation around y-axis
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f); // Apply to player transform
        
    }

    private void LookInfo()
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
