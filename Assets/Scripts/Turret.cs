using System;
using System.Collections;
using DevScripts;
using UnityEngine;
using Random = UnityEngine.Random;

// Describe class function here

public class Turret : MonoBehaviour, IDamageable
{
    
    [Header("References")]
    [SerializeField] private Collider player;
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject barrel;
    [SerializeField] private Transform firePoint;
    
    private float _playerDistance;
    private Vector3 _playerDir;
    private bool _obstructed;
    
    
    [Header("Combat")]
    [SerializeField] private int maxHealth;
    private int _health;
    
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject bullet;
    
    [SerializeField] private float detectRange;
    [SerializeField] private float attackRange;
    
    
    [Header("Patrol")]
    [Tooltip("Time spent patrolling before returning to idle")] 
    [SerializeField] private float patrolTime;
    [Tooltip("Time spent looking at player's last location")] 
    [SerializeField] private float initialWaitTime;
    [Tooltip("Time spent waiting at left or rightmost position")] 
    [SerializeField] private float waitTime; 
    [SerializeField] private float patrolTurnRate;
    [Tooltip("Angle range the turret checks while patrolling")] 
    [SerializeField] private float patrolAngle;
    
    private Coroutine _patrolRoutine;
    private float _startAngle;
    private float _count;
    
    
    [Header("Turning")]
    [SerializeField] private float platformTurnRate;
    [SerializeField] private float barrelTurnRate;
    [Tooltip("How close to the player the turret needs to be aiming before it starts shooting")]
    [SerializeField] private float shootAngle;
    
    [Tooltip("How far down the turret barrel can look")]
    [SerializeField] private float minAngle;
    [Tooltip("How far up the turret barrel can look")]
    [SerializeField] private float maxAngle;
    
    private bool _lookingAtPlayer;

    private enum TurretState
    {
        Idle,
        Patrol,
        Ready,
        Attack
    }
    
    private TurretState _state = TurretState.Idle;
    
    
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
            // If hit object belongs to the turret, ignore it
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

    IEnumerator Patrol()
    {
        /* Patrol behaviour should work as such:
         1. When the player leaves range, the turret looks in their last seen direction for x time
         2. Then, it constantly looks left and right within X degrees of that direction, with a small stop at the leftmost/rightmost position
         */
        
        var startAngle = platform.transform.localEulerAngles.y;
        
        yield return new WaitForSeconds(initialWaitTime);
        
        var currentAngle = platform.transform.localEulerAngles.y;
        var nextAngle = Mathf.MoveTowardsAngle(currentAngle, startAngle - patrolAngle, patrolTurnRate * Time.deltaTime);
        var clampedAngle = Mathf.Clamp(nextAngle, startAngle -patrolAngle, startAngle + patrolAngle);
        
        platform.transform.rotation = Quaternion.Euler(0, clampedAngle, 0);
        
        yield return new WaitForSeconds(waitTime);
        
        
    }
    
    
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
        
        
        
        
        _count -= Time.deltaTime; // Time spent in patrol state
        
        
        // Transition to idle state
        if (_count <= 0)
        {
            _state = TurretState.Idle;
            Debug.Log(_state);
        }
        
        // Transition to ready state
        if (!_obstructed && _playerDistance <= detectRange)
        {
            StopCoroutine(_patrolRoutine);
            _state = TurretState.Ready;
            Debug.Log(_state);
        }
        
    }


    private void ReadyBehaviour()
    {
        
        PlatformRotate();
        BarrelRotate();
        
        // Transition to patrol state
        if (_obstructed || _playerDistance > detectRange)
        {
            _count = patrolTime;
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
        
        // Transition to ready state
        if (_playerDistance > attackRange)
        {
            _state = TurretState.Ready;
            Debug.Log(_state);
        }
        
    }


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

    void Shoot()
    {
        
        
        
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
