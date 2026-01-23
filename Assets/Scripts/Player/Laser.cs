using UnityEngine;
using UnityEngine.InputSystem;

// Laser weapon that pushes RBs around and deals damage

public class Laser : MonoBehaviour
{
    
    [SerializeField] private float DPS;
    [SerializeField] private float pushForce;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject laserBeam;
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
        
        
        // Input system //
        _shootAction = InputSystem.actions.FindAction("Attack");
        
        // Subscribe to inputSystem events so they can change isHeldDown var
        _shootAction.started += ctx => isHeldDown = true;
        _shootAction.canceled += ctx => isHeldDown = false;
        
        laserBeam.SetActive(false);

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
            laserBeam.SetActive(true);
            laserActive = true;
            
        }
        else if (!isHeldDown && laserActive)
        {
            laserBeam.SetActive(false);
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
