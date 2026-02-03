using System;
using System.Collections;
using System.Collections.Generic;
using DevScripts;
using UnityEngine;

// Turret with FSM and object pooling

public class Turret : MonoBehaviour, IDamageable
{
    
    [Header("References")] // HEADER
    [SerializeField] private Collider player;
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject barrel;
    [SerializeField] private Transform firePoint;

    [Header("Modifiers")] // HEADER
    [SerializeField] private ShootType shootType;
    [SerializeField] private MoveType moveType;
    [SerializeField] private List<EffectType> effectType;
    [SerializeField] private ElementType elementType;
    
    private float _playerDistance;
    private Vector3 _playerDir;
    private bool _obstructed;
    
    
    
    
    [Header("Combat")] // HEADER
    [SerializeField] private int maxHealth;
    private int _health;
    
    [Tooltip("Bullets per second")] 
    [SerializeField] private float fireRate;
    private Coroutine _shootRoutine;
    private WaitForSeconds _shootWait;
    
    [Tooltip("Range at which turret enters ready state")] 
    [SerializeField] private float detectRange;
    [Tooltip("Range at which turret begins attacking")] 
    [SerializeField] private float attackRange;
    
    
    
    [Header("Patrol")] // HEADER
    [Tooltip("Time spent looking at player's last location")] 
    [SerializeField] private float initialWaitTime;
    [Tooltip("Time spent waiting at left or rightmost position")] 
    [SerializeField] private float waitTime; 
    [SerializeField] private float patrolTurnRate;
    [Tooltip("Angle range the turret checks while patrolling")] 
    [SerializeField] private float patrolAngle;
    
    private Coroutine _patrolRoutine;
    private WaitForSeconds _patrolWait;
    
    
    [Header("Turning")] // HEADER
    [SerializeField] private float platformTurnRate;
    [SerializeField] private float barrelTurnRate;
    [Tooltip("How close to the player the turret needs to be aiming before it starts shooting")]
    [SerializeField] private float shootAngle;
    
    [Tooltip("How far down the turret barrel can look")]
    [SerializeField] private float minAngle;
    [Tooltip("How far up the turret barrel can look")]
    [SerializeField] private float maxAngle;
    
    
    
    [Header("SFX")] // HEADER
    
    
    
    [Header("VFX")] // HEADER
    [SerializeField] private GameObject muzzleVFX;
    [Range(0f, 1f)][SerializeField] private float muzzleVFXSize;
    private Vector3 _muzzleVFXSize;
    
    
    private bool _lookingAtPlayer;
    
    
    private enum TurretState
    {
        Idle,
        Patrol,
        Ready,
        Attack
    }
    
    private TurretState _state = TurretState.Idle;

    void Start()
    {
        _patrolWait = new WaitForSeconds(waitTime);
        _shootWait = new WaitForSeconds(1/fireRate);
        
        _muzzleVFXSize = new Vector3(muzzleVFXSize, muzzleVFXSize, muzzleVFXSize);
    }
    
    void Update()
    {
        PlayerInfo();
        ShootRay();
        
        SwitchState();
        
    }

    private void ShootRay()
    {
        // Ray origin is the body position
        var rayObjects = Physics.RaycastAll(barrel.transform.parent.position, _playerDir, _playerDistance);
        
        Array.Sort(rayObjects, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var obj in rayObjects)
        {
            // If hit object belongs to the turret, ignore it and move to next obj
            if (obj.transform.IsChildOf(transform))
                continue;
            
            _obstructed = obj.transform != player.transform;
            //Debug.Log(_obstructed);

            break;

        }
        
        Debug.DrawRay(barrel.transform.parent.position, _playerDir * _playerDistance, Color.red);
        
        
    }

    private void PlayerInfo()
    {
        // All calcs use the body transform as the origin 
        _playerDir = (player.transform.position - barrel.transform.parent.position).normalized;
        _playerDistance = Vector3.Distance(player.transform.position, barrel.transform.parent.position);
    }

    
    
    #region State Machine
    private void SwitchState()
    {

        switch (_state)
        {
            case TurretState.Idle:
                
                IdleBehaviour();
                
                break;
            
            case TurretState.Patrol:

                PatrolBehaviour();
                
                break;
            
            case TurretState.Ready:
                
                ReadyBehaviour();
                
                break;
            
            case TurretState.Attack:
                
                AttackBehaviour();
                
                break;
        }
    }

    private void IdleBehaviour()
    {
        // Make it so barrel moves down to minAngle
        var nextAngle = Mathf.MoveTowardsAngle(
            barrel.transform.eulerAngles.x, -minAngle, barrelTurnRate * Time.deltaTime);
        
        barrel.transform.localRotation = Quaternion.Euler(nextAngle, 0, 0);
        
        // Transition to ready state
        if (!_obstructed && _playerDistance <= detectRange)
        {
            _state = TurretState.Ready;
            Debug.Log(_state);
        }
    }
    

    private void PatrolBehaviour()
    {
        // Transition to ready state
        if (!_obstructed && _playerDistance <= detectRange)
        {
            StopCoroutine(_patrolRoutine);
            _state = TurretState.Ready;
            Debug.Log(_state);
        }
    }
    
    
    IEnumerator Patrol()
    {
        /* Patrol behaviour should work as such:
         1. When the player leaves range, the turret looks in their last seen direction for x time
         2. Then, it constantly looks left and right within X degrees of that direction, with a small stop at the leftmost/rightmost position
         */
        
        var startAngle = platform.transform.localEulerAngles.y;
        var leftAngle = startAngle - patrolAngle;
        var rightAngle = startAngle + patrolAngle;
        
        // Look in last seen direction for x seconds
        yield return new WaitForSeconds(initialWaitTime);
        
        // Start rotating left
        yield return RotateToAngle(leftAngle);
        yield return _patrolWait;
        
        // Start rotating right
        yield return RotateToAngle(rightAngle);
        yield return _patrolWait;
        
        // Start rotating left
        yield return RotateToAngle(leftAngle);
        yield return _patrolWait;
        
        // Start rotating right
        yield return RotateToAngle(rightAngle);
        yield return _patrolWait;
        
        // Rotate back to lastseen dir
        yield return RotateToAngle(startAngle);


        _state = TurretState.Idle;
        Debug.Log(_state);
    }
    
    // Handles rotation inside Patrol routine - needs to be IEnum instead of method
    IEnumerator RotateToAngle(float targetAngle)
    {
        while (Mathf.Abs(Mathf.DeltaAngle(platform.transform.localEulerAngles.y, targetAngle)) > 0.1f)
        {
            var currentAngle = platform.transform.localEulerAngles.y;
            
            var nextAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, patrolTurnRate * Time.deltaTime);
            
            platform.transform.localRotation = Quaternion.Euler(0, nextAngle, 0);
            yield return null;
        }
    }

    
    private void ReadyBehaviour()
    {
        PlatformRotate();
        BarrelRotate();
        
        // Transition to patrol state
        if (_obstructed || _playerDistance > detectRange)
        {
            _patrolRoutine = StartCoroutine(Patrol());
            _state = TurretState.Patrol;
            Debug.Log(_state);
        }
        
        // Transition to attack state
        else if (_playerDistance <= attackRange)
        {
            _state = TurretState.Attack;
            
            Debug.Log(_state);
        }
    }


    private void AttackBehaviour()
    {
        PlatformRotate();
        BarrelRotate();
        if (_shootRoutine != null)
        {
            _shootRoutine = StartCoroutine(Shoot());
        }
        
        
        // Transition to ready state
        if (_playerDistance > attackRange || _obstructed)
        {
            _state = TurretState.Ready;
            
            Debug.Log(_state);
        }
    }
    #endregion

    
    void PlatformRotate()
    {
        TransformUtils.RotateAt(platform,player.transform.position, platformTurnRate, transform.up);
    }
    
    // It fucking works!!! Utter bullshit that it took this long

    void BarrelRotate()
    {
        var target = player.transform.position;
        var targetDir = (barrel.transform.position - target).normalized;
        var targetRotation =  Quaternion.LookRotation(targetDir);
        
        // Keeps angle within a -180° to 180° range (instead of 0-360), allowing the angle clamping to work correctly
        float targetAngle = -targetRotation.eulerAngles.x;
        
        if (targetAngle < -180) targetAngle += 360;
        
        // Change rotation by turn rate
        var currentAngle = barrel.transform.localEulerAngles.x;
        if (currentAngle > 180) currentAngle -= 360;
        
        var nextAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, barrelTurnRate * Time.deltaTime);
        
        float clampedAngle = Mathf.Clamp(nextAngle, -maxAngle, -minAngle);
        barrel.transform.localRotation = Quaternion.Euler(clampedAngle, 0, 0);
    }

    IEnumerator Shoot()
    {
        
        // Muzzle VFX
        var vfx = Instantiate(muzzleVFX, firePoint.position, firePoint.rotation);
        vfx.transform.parent = firePoint.transform;
        vfx.transform.localScale = _muzzleVFXSize;
        Destroy(vfx, 1);

        if (shootType.Routine != null)
        {
            shootType.Routine = StartCoroutine(shootType.ShootRoutine(firePoint, shootType, moveType, effectType, elementType));
        }
        
        yield return _shootWait;

    }


    public void TakeDamage(int damage)
    {
        _health -= damage;
    }

    public void OnDestroy()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    
    
    
// Class ends here
}
