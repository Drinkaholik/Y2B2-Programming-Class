using UnityEngine;
using UnityEngine.InputSystem;

public class TowerThrust : MonoBehaviour
{
    
    [SerializeField] private GameObject tower;
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;


    private Vector3 startPos;
    private Vector3 nextPos;
    private Vector3 endPos;
    private Vector3 targetPos;
    
    private bool moveUp;

    private InputAction _interactAction;
    private Rigidbody rb;
    
    void Start()
    {
        
        _interactAction = InputSystem.actions.FindAction("Interact");
        
        rb = GetComponent<Rigidbody>();
        
        startPos = transform.position;
        targetPos = startPos;
        endPos = new Vector3(transform.position.x, transform.position.y + moveDistance, transform.position.z);
        
        
        
    }

    
    
    // Update is called once per frame
    void Update()
    {
        
        
        if (_interactAction.triggered)
        { 
            moveUp = !moveUp;
            targetPos = moveUp ? endPos : startPos;
            
        }

        
    }

    void FixedUpdate()
    {
        
        if (rb.position != targetPos)
        {
            // Lerp between current position and target position
            nextPos = Vector3.Lerp(rb.position, targetPos, moveSpeed * Time.deltaTime);
        
            rb.MovePosition(nextPos);
        }
        
        
    }
    
    
}
