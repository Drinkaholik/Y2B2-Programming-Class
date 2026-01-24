using UnityEngine;
using UnityEngine.InputSystem;

// Laser weapon that pushes RBs around and deals damage

public class Laser : MonoBehaviour
{
    
    [SerializeField] private float DPS;
    [SerializeField] private float pushForce;
    [SerializeField] private Transform firePoint;
    private LineRenderer _line;
    private bool laserActive;
    
    // Components
    private Charge charge; // Charge component
    private Push push; // Push component
    private Spin spin;
    
    private InputAction _shootAction;
    private bool isHeldDown;
    
    void Start()
    {
        // Link up components
        charge = GetComponent<Charge>();
        push = GetComponent<Push>();
        spin = GetComponent<Spin>();
        
        _line = GetComponent<LineRenderer>();
        _line.enabled = false;
        _line.positionCount = 2;    
        
        
        // Input system //
        _shootAction = InputSystem.actions.FindAction("Attack");
        
        // Subscribe to inputSystem events so they can change isHeldDown var
        _shootAction.started += ctx => isHeldDown = true;
        _shootAction.canceled += ctx => isHeldDown = false;
        

    }
    
    
    void Update()
    {
        
        if (isHeldDown)
        {
            Shoot();
            
        } 
        LaserBeam(); 
        Charge();
        Spin();
        
    }

    private void Shoot()
    {
        if (PlayerLook.lookObject == null) return;
        push.PushObject(PlayerLook.lookObject);
        
    }

    private void LaserBeam()
    {
        if (isHeldDown && !laserActive)
        {
            _line.enabled = true;
            _line.SetPosition(0, firePoint.position);
            _line.SetPosition(1, PlayerLook.lookPosition);
            laserActive = true;
            
        }
        else if (!isHeldDown && laserActive)
        {
            _line.enabled = false;
            laserActive = false;
        }
        
        
    }

    private void Charge()
    {
        if (isHeldDown)
        {
            charge.Charging();
        }
        else
        {
            charge.Discharging();
        }
        
    }
    
    private void Spin()
    {
        
        if (isHeldDown)
        {
            spin.SpinUp();
        }
        else
        {
            spin.SpinDown();
        }
        
        spin.SpinObject();
        
    }
    
// Class ends here
}
