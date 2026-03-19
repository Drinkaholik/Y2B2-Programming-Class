using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Injects modifiers into bullets as it shoots
// Has no stats on its own - that's handled by ShootType

public class Gun : MonoBehaviour
{

    [Header("Combat")] 
    [SerializeField] private Transform firePoint;
    [SerializeField] private bool isBurst;
    
    [Header("Modifiers")] 
    public ShootType shootType;
    public MoveType moveType;
    public List<EffectType> effects = new();
    public ElementType elementType;


    [Header("Visuals")] 
    [SerializeField] private Material baseMat;
    private Material _currentMat;
    private Material[] _materials;
    private Renderer _renderer;
    
    
    private InputAction _shootAction;
    private bool _tryingShoot;
    private float _count;
    
    
    void Start()
    {
        _shootAction = InputSystem.actions.FindAction("Attack");
        _shootAction.performed += ctx => _tryingShoot = true;
        _shootAction.canceled += ctx => _tryingShoot = false;
        
        _renderer = GetComponent<Renderer>();
        _materials = _renderer.materials;
        _currentMat = elementType.material;
    }
    
    
    void Update()
    {
        Shoot();
        
        // Temp, delete once pickup system works
        if (_renderer.materials[2] != elementType.material)
        {
            _currentMat = elementType.material;
            _materials[2] = _currentMat;
            _renderer.materials  = _materials;
        }
            
        
    }

    void Shoot()
    {
        if (_tryingShoot && _count <= 0)
        {
            shootType.Shoot(firePoint, shootType, moveType, effects, elementType);
            _count = shootType.fireRateCount;
        }
        _count -= Time.deltaTime;
        
    }
    
    // Only used to change the gun's materials to match that of the element
    public void OnPickup(ElementType element)
    {
        elementType = element;
        
        // Set material - have to replace entire array, kinda cringe
        _currentMat = elementType.material;
        _materials[2] = _currentMat;
        _renderer.materials  = _materials;
        
    }
    
    
// Class ends here
}
