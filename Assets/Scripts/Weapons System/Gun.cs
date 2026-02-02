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
    
    [Header("Modifiers")] 
    public ShootType shootType;
    public MovementType moveType;
    public HashSet<EffectType> effects = new();
    public ElementType elementType;


    [Header("Visuals")] 
    [SerializeField] private Material baseMat;
    private Material _currentMat;
    private Material[] _materials;
    private Renderer _renderer;
    
    
    private InputAction _shootAction;
    private bool _tryingShoot;
    
    void Start()
    {
        _shootAction = InputSystem.actions.FindAction("Attack");
        _shootAction.performed += ctx => _tryingShoot = true;
        _shootAction.canceled += ctx => _tryingShoot = false;
        
        _renderer = GetComponent<Renderer>();
        _materials = _renderer.materials;
        _currentMat = elementType.elementMaterial;
    }
    
    
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (_tryingShoot && shootType.Routine == null)
        {
            shootType.Routine = StartCoroutine(shootType.ShootRoutine(bulletPool,firePoint, this, _currentMat));
        }
            
    }

    void OnPickup()
    {
        
        if (elementType != null)
        {
            _currentMat = elementType.elementMaterial;
            _materials[2] = _currentMat;
            _renderer.materials  = _materials;
        }
    }
    
    
// Class ends here
}
