using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Injects modifiers into bullets as it shoots
// Has no stats on its own - that's handled by ShootType

public class Gun : MonoBehaviour
{

    [Header("Combat")] 
    [SerializeField] private Transform firePoint;
    public BulletPool bulletPool;
    
    // 4 types of bullet modifiers
    public ShootType shootType;
    public MovementType moveType;
    public HashSet<EffectType> effects = new();
    public ElementType elementType;

    private InputAction _shootAction;
    private bool _tryingShoot;
    //private float _timer; // Needed for timing burst fire - didn't work with double coroutine
    
    void Start()
    {
        _shootAction = InputSystem.actions.FindAction("Attack");
        _shootAction.performed += ctx => _tryingShoot = true;
        _shootAction.canceled += ctx => _tryingShoot = false;
    }
    
    
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (_tryingShoot && shootType.Routine == null)
        {
            shootType.Routine = StartCoroutine(shootType.ShootRoutine(bulletPool,firePoint, this));
            
        }
            
    }
    
    
// Class ends here
}
