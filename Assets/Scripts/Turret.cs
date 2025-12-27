using DevScripts;
using UnityEditor.ShaderGraph;
using UnityEngine;

// Describe class function here

public class Turret : MonoBehaviour
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
    private Vector3 _randomDir;
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
        _randomDir = new Vector3(0, Random.Range(0, 360), 0);
    }
    
    void Update()
    {
        SwitchState();
        
        ShootRay();
        
        _playerDistance = Vector3.Distance(player.transform.position, transform.position);
        _playerDir = (player.transform.position - transform.position).normalized;
        
        Debug.Log(_state.ToString());
    }

    private void ShootRay()
    {
        
        Physics.Raycast(transform.position, _playerDir, out RaycastHit hit);
        Debug.DrawRay(transform.position, _playerDir * hit.distance, Color.red);

        _playerDistance = hit.distance;
        _obstructed = hit.collider != player;
        
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
        
        
        // Transition to ready state

        if (!_obstructed && _playerDistance <= detectRange)
        {
            
            _state = TurretState.Ready;
            
        }
        
    }
    

    private void PatrolBehaviour()
    {
        
        _count -= Time.deltaTime; // Time spent in patrol state

        if (transform.rotation.eulerAngles == _randomDir)
        {
            _randomDir = new Vector3(0, Random.Range(0, 360), 0);
            
        }
        else
        {
            TransformUtils.RotateObject(platform,player.transform.position, platformTurnRate, Vector3.up);
        }
        
        
        
        // Transition to idle state
        if (_count <= 0)
        {
            _state = TurretState.Idle;
        }
        
    }


    private void ReadyBehaviour()
    {
        
        TransformUtils.RotateObject(platform,player.transform.position, platformTurnRate, Vector3.up);
        
        // Transition to patrol state
        if (_obstructed || _playerDistance > detectRange)
        {
            _count = patrolTime;
            _state = TurretState.Patrol;
        }
        
        // Transition to attack state
        else if (_playerDistance <= attackRange)
        {
            
            _state = TurretState.Attack;
        }
        
    }


    private void AttackBehaviour()
    {
        
        TransformUtils.RotateObject(platform,player.transform.position, platformTurnRate, Vector3.up);
        TransformUtils.RotateObject(barrel,player.transform.position, barrelTurnRate, Vector3.right);
        
        // Transition to ready state
        if (_playerDistance > attackRange)
        {
            _state = TurretState.Ready;
        }
        
    }
    
    
    
    
// Class ends here
}
