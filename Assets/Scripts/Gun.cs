using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Handles firerate and shooting logic

public class Gun : MonoBehaviour
{

    [Header("Combat")] 
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    
    // 4 types of bullet modifiers
    private ShootType _shootType;
    private MovementType _movementType;
    private HashSet<EffectType> _effectType;
    private ElementType _elementType;

    private InputAction _shootAction;
    private bool _tryingShoot;
    
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
        if (_tryingShoot && _shootType.Routine == null)
            _shootType.Routine = StartCoroutine(_shootType.ShootRoutine(bullet, firePoint));
    }
    
    
// Class ends here
}
