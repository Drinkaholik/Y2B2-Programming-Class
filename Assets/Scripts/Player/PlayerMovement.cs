using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    [Header("Visuals")]
    [SerializeField] private GameObject capsule;
    
    private Camera cam;
    private float cameraOffset; // Offset camera position downward while crouching
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private float playerSpeed;
    private Vector3 finalMove;
   
    private CharacterController cc;
    
    [Header("Jumping")]
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForce;
    private bool isJumping;
    private float yMove;
    
    private float groundDistance;
    
    
    [Header("Crouching")]
    [SerializeField] private float crouchSpeed; // Move speed while crouching
    [SerializeField] private float crouchRate; // Rate that height lerps between crouch and norm values
    [SerializeField] private float crouchHeight;
    private float normHeight;
    
    
    private bool isCrouching;
    
    
    // Input actions
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _crouchAction;
    
    
    void Start()
    {
        // Character controller values
        cc = GetComponent<CharacterController>();
        normHeight = cc.height;
        
        cam = Camera.main;
        cameraOffset = cam.transform.localPosition.y;
        
        Cursor.lockState = CursorLockMode.Locked;
        
        // Find references to the input action
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _crouchAction = InputSystem.actions.FindAction("Crouch");
        
    }

    // Update is called once per frame
    void Update()
    {
        
        PlayerMove();

        PlayerGrav();
        
        PlayerJump();
        
        PlayerCrouch();
        

        finalMove.y = yMove;
        cc.Move(finalMove * Time.deltaTime);
        
    }


    private bool IsGrounded()
    {
        
        // Setting up raycast to ignore a layer //
        // Each layer is represented by one of the bits on a 32 bit integer, where layer 1 is ...00001, and layer 2 is ...00010
        // LayerMasks in the raycast use signed integers, so to define a mask to use or ignore, you need to use signed integers
        // << is a 'left-shift bitwise operator'. It takes the binary value of 1, which is ...00001, and shifts it 2 places to the left, leaving you with ...00100
        // This is the binary value for the "Ignore Raycast" layer
        // Since a mask defines which layers to interact with, you then need to invert this binary result using the bitwise NOT operator
        // This leaves you with ...111011, which means the "Ignore Raycast" layer will not interact with the ray
        LayerMask ignoreLayer = 1 << LayerMask.NameToLayer("Ignore Raycast");
        LayerMask interactMask = ~ignoreLayer; // ~ is the bitwise NOT operator
        
        var ray = Physics.SphereCast(transform.position, cc.radius, Vector3.down, out RaycastHit hit, Mathf.Infinity, interactMask);
        
        groundDistance = hit.distance;
        
        // Checks if groundDistance is less than player height / 2
        return (groundDistance <= (normHeight/2) - cc.radius + 0.2f);
    }
    
    private void PlayerMove()
    {
        
        // WASD Movement
        var moveVector = _moveAction.ReadValue<Vector2>();
        
        var moveDir = new Vector3(moveVector.x, 0, moveVector.y);
        
        var localDir = transform.TransformDirection(moveDir) * playerSpeed;;
        
        finalMove.x = localDir.x;
        finalMove.z = localDir.z;
        
        
    }


    private void PlayerGrav()
    {
        
        if (!IsGrounded())
        {
            yMove -= gravity *  Time.deltaTime;
        }
        
        if (IsGrounded() && !isJumping)
        {
            yMove = 0;
        }
        
    }


    private void PlayerJump()
    {
        
        if (_jumpAction.triggered && IsGrounded())
        {
           
            yMove = jumpForce;
            
            isJumping = true;
            
        }
        
        
    }

    private void PlayerCrouch()
    {
        
        if (_crouchAction.triggered)
        {
            isCrouching = !isCrouching;
        }
        
        if (isCrouching)
        {
            playerSpeed = crouchSpeed;
            
            cc.height = Mathf.Lerp(cc.height, crouchHeight, crouchRate * Time.deltaTime);
            
            
             
            
        }
        else
        {
            playerSpeed = moveSpeed;
            
            cc.height = Mathf.Lerp(cc.height, normHeight, crouchRate  * Time.deltaTime);
        }
        
        // Need to move centre of cc so that bottom of capsule maintains position while crouching
        var newCentre = (cc.height - normHeight) / 2;
        cc.center = new Vector3 (0, newCentre, 0); 
        
        capsule.transform.localScale = new Vector3(1, cc.height / 2, 1);  // Change capsule visuals

        var camY = cameraOffset + (cc.height - normHeight);
        
        cam.transform.localPosition = new Vector3(0, camY, 0);
     
        
    }
    
    
    
    
   

    
    
    
    
}
