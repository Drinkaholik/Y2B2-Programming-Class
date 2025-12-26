using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Kick : MonoBehaviour
{
    
    [SerializeField] private float kickForce;


    private Camera cam;
    
    private Vector3 kickPoint = new(0, -1, 0);
    
    private List<GameObject> kickable = new();
    
    private InputAction _kickAction;
    
    void Start()
    {
        
        _kickAction = InputSystem.actions.FindAction("Kick");
        
        cam = Camera.main;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_kickAction.triggered)
        {
            KickObjects();
        
        }
        
    }


    private void KickObjects()
    {
        // Loop through kickable list
        foreach (var obj in kickable)
        {
            
            var rb = obj.GetComponent<Rigidbody>();
            var kickDir = Vector3.Normalize(rb.position - (transform.position + kickPoint));
            
            rb.AddForce(kickDir * kickForce);
            
        }
    }
    
    
    
    // Add rigidbodies to list if they enter kick zone
    void OnTriggerEnter(Collider other)
    {
        kickable.Add(other.gameObject);
    }
    
    // Remove if they exit kickzone
    void OnTriggerExit(Collider other)
    {
        kickable.Remove(other.gameObject);
    }
    
}
