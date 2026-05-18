using System;
using System.Collections.Generic;
using DevScripts;
using UnityEngine;

// Describe class function here

public class TurretContext : MonoBehaviour
{
    
    private TurretState _currentState;
    
    [HideInInspector] public TurretIdle idleState;
    [HideInInspector] public TurretPatrol patrolState;
    [HideInInspector] public TurretReady readyState;
    [HideInInspector] public TurretAttack attackState;
    
    
    [Header("References")] // HEADER
    [HideInInspector] public Collider player;
    public GameObject platform;
    public GameObject barrel;
    public Transform firePoint;

    
    [Header("Modifiers")] // HEADER
    public ShootType shootType;
    public MoveType moveType;
    public List<EffectType> effects;
    public ElementType elementType;
    
    
    [HideInInspector] public float playerDistance;
    [HideInInspector] public Vector3 playerDir;
    [HideInInspector] public bool obstructed;
    
    
    [Header("Combat")] // HEADER
    [SerializeField] private int maxHealth;
    [HideInInspector] public int health;
    [Tooltip("Range at which turret enters ready state")] 
    public float detectRange;
    [Tooltip("Range at which turret begins attacking")] 
    public float attackRange;
    
    [Header("Patrol")] // HEADER
    [Tooltip("Number of times the turret will turn left and right")] 
    public int timesTurned = 1;
    [Tooltip("Time spent looking at player's last location")] 
    public float initialWaitTime;
    [Tooltip("Time spent waiting at left or rightmost position")] 
    public float waitTime; 
    public float patrolTurnRate;
    [Tooltip("Angle range the turret checks while patrolling")] 
    public float patrolAngle;
    
    
    [Header("Turning")] // HEADER
    public float platformTurnRate;
    public float barrelTurnRate;
    
    [Tooltip("How far down the turret barrel can look")]
    public float minAngle;
    [Tooltip("How far up the turret barrel can look")]
    public float maxAngle;
    
    
    [Header("Visuals")] // HEADER
    [SerializeField] private GameObject muzzleVFX;
    [Range(0f, 1f)] [SerializeField] private float muzzleVFXSize;
    private Vector3 _muzzleVFXVector;

    [SerializeField] private GameObject tLight;
    [Range(0f, 10f)] public float intensity = 1;



    [HideInInspector] public Material lightMat;
    public readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    
    
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Collider>();
        lightMat = tLight.GetComponent<Renderer>().material;
        
        idleState = gameObject.AddComponent<TurretIdle>();
        patrolState = gameObject.AddComponent<TurretPatrol>();
        readyState = gameObject.AddComponent<TurretReady>();
        attackState = gameObject.AddComponent<TurretAttack>();
        
        ChangeState(idleState);
        
        _muzzleVFXVector = new Vector3(muzzleVFXSize, muzzleVFXSize, muzzleVFXSize);
        
    }

    void Update()
    {
        if (player != null)
        {
            PlayerInfo();
            ShootRay();
        }
        
        _currentState.Tick();
    }
    
    

    public void ChangeState(TurretState newState)
    {
        // Exit old state
        if (_currentState != null)
        {
            _currentState.OnExitState();
            _currentState.enabled = false;
        }
        
        // Enter new state
        _currentState = newState;
        _currentState.enabled = true;
        _currentState.OnEnterState(); 
    }
    
    
    protected void TakeDamage(int damage)
    {
        health -= damage;
    }

    protected void OnDestroy()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    void ShootRay()
    {
        // Ray origin is the body position
        var rayObjects = Physics.RaycastAll(barrel.transform.parent.position, playerDir, playerDistance);
        
        Array.Sort(rayObjects, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var obj in rayObjects)
        {
            // If hit object belongs to the turret, ignore it and move to next obj
            if (obj.transform.IsChildOf(transform))
                continue;
            
            obstructed = obj.transform != player.transform;
            //Debug.Log(_obstructed);

            break;

        }
        
        Debug.DrawRay(barrel.transform.parent.position, playerDir * playerDistance, Color.red);
        
    }

    void PlayerInfo()
    {
        // All calcs use the body transform as the origin 
        playerDir = (player.transform.position - barrel.transform.parent.position).normalized;
        playerDistance = Vector3.Distance(player.transform.position, barrel.transform.parent.position);
    }
    
    
    
// Class ends here
}


public abstract class TurretState : MonoBehaviour
{
    
    public TurretContext context;
    
    public abstract void OnEnterState();

    protected abstract void StateBehaviour();
    
    public abstract void OnExitState();

    protected abstract void Transition();
    
    public abstract void Tick();
    

    
    
    protected void PlatformRotate()
    {
        TransformUtils.RotateAt(context.platform,context.player.transform.position, context.platformTurnRate, transform.up);
    }
    

    protected void BarrelRotate()
    {
        var target = context.player.transform.position;
        var targetDir = (context.barrel.transform.position - target).normalized;
        var targetRotation =  Quaternion.LookRotation(targetDir);
        
        // Keeps angle within a -180° to 180° range (instead of 0-360), allowing the angle clamping to work correctly
        float targetAngle = -targetRotation.eulerAngles.x;
        
        if (targetAngle < -180) targetAngle += 360;
        
        // Change rotation by turn rate
        var currentAngle = context.barrel.transform.localEulerAngles.x;
        if (currentAngle > 180) currentAngle -= 360;
        
        var nextAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, context.barrelTurnRate * Time.deltaTime);
        
        float clampedAngle = Mathf.Clamp(nextAngle, -context.maxAngle, -context.minAngle);
        context.barrel.transform.localRotation = Quaternion.Euler(clampedAngle, 0, 0);
    }

   

    protected void SetColour(Color colour)
    {
        context.lightMat.color = colour;
        context.lightMat.SetColor(context.EmissionColor, colour * Mathf.Pow(2, context.intensity));
        context.lightMat.EnableKeyword("_EMISSION");
    }
}
