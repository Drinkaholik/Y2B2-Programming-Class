using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Kick : MonoBehaviour
{
    
    [SerializeField] private float kickForce;

    
    private Vector3 _kickPoint = new(0, -1, 0);
    
    private List<GameObject> _kickable = new();
    
    private InputAction _kickAction;
    
    void Start()
    {
        
        _kickAction = InputSystem.actions.FindAction("Kick");
        
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
        // Incase object is destroyed before leaving kickZone
        _kickable.RemoveAll(item => item == null);
        
        // Loop through kickable list
        foreach (var obj in _kickable)
        {
            if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                var kickDir = Vector3.Normalize(rb.position - (transform.position + _kickPoint));
            
                rb.AddForce(kickDir * kickForce);
            }
        }
    }
    
    
    
    // Add rigidbodies to list if they enter kick zone
    void OnTriggerEnter(Collider other)
    {
        _kickable.Add(other.gameObject);
    }
    
    // Remove if they exit kickzone
    void OnTriggerExit(Collider other)
    {
        _kickable.Remove(other.gameObject);
    }
    
}
