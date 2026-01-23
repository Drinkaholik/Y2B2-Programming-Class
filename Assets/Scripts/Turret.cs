using System;
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
    
    [SerializeField] private int damage;
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bullet;
    
    [SerializeField] private float detectRange;
    [SerializeField] private float attackRange;
    
    
    [Header("Patrol")]
    [Tooltip("Time in seconds spent patrolling before returning to idle")] 
    [SerializeField] private float patrolTime;
    [Tooltip("Time in seconds spent waiting before rotating to new direction")] 
    [SerializeField] private float waitTime; 
    [SerializeField] private float patrolTurnRate;
    private Vector3 _randomPos;
    private Vector3 _lastSeen; // Players last seen location
    private float _count;
    
    //private Coroutine _patrolCoroutine;
    
    [Header("Turning")]
    [SerializeField] private float platformTurnRate;
    [SerializeField] private float barrelTurnRate;
    [Tooltip("How close to the player the turret needs to be aiming before it starts shooting")]
    [SerializeField] private float shootAngle;
    
    [Tooltip("How far down the turret barrel can look")]
    [SerializeField] private float minAngle;
    [Tooltip("How far up the turret barrel can look")]
    [SerializeField] private float maxAngle;

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
        _randomPos = new Vector3(0, Random.Range(0, 360), 0);
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

        var range = 10;
        
        // Find new rotation if 
        if (platform.transform.forward == (_randomPos - platform.transform.position).normalized)
        {
            // New rotation is based on player's last seen position
            _randomPos = new Vector3(_lastSeen.x + Random.Range(-range, range), 0, _lastSeen.z + Random.Range(-range, range));
            
        }
        else
        {
            TransformUtils.RotateAt(platform, _randomPos, platformTurnRate, transform.up);
        }
        
        
        // Transition to idle state
        if (_count <= 0)
        {
            _state = TurretState.Idle;
            Debug.Log(_state);
        }
        
        // Transition to ready state
        if (!_obstructed && _playerDistance <= detectRange)
        {
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
            _lastSeen = player.transform.position;
            _count = patrolTime;
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
        float angle = -targetRotation.eulerAngles.x;
        if (angle < -180) angle += 360;
        float clampedAngle = Mathf.Clamp(angle, minAngle, maxAngle);
        barrel.transform.localRotation = Quaternion.Euler(clampedAngle, 0, 0);
    }


    public void TakeDamage(int damage)
    {
        _health -= damage;
    }
    
    
    
    
// Class ends here
}
