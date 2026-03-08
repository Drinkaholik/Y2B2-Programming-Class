using UnityEngine;

// Holds data and methods for the bullet elements
// All elements are mutually exclusive

[CreateAssetMenu(fileName = "ElementType", menuName = "Scriptable Objects/ElementType")]
public class ElementType : ModifierType
{

    public enum ElementBehaviour
    {
        None,
        Freeze,
        Burn
    }
    
    // HideInInspector is necessary for the custom inspector script
    public ElementBehaviour behaviour;
    [HideInInspector] public Material material;
    [HideInInspector] public Material trailMaterial;
    
    [Header("Default Stats")]
    [HideInInspector] public Material baseMaterial;
    [HideInInspector] public Material baseTrailMaterial;
    
    [Header("Freeze Stats")]
    [Tooltip("Amount to chill the target by. When they reach 100%, they freeze.")]
    [HideInInspector] [SerializeField] private int freezeAmount;
    [Tooltip("Defines gun and bullet colour.")]
    [HideInInspector] [SerializeField] private Material freezeMaterial;
    [HideInInspector] [SerializeField] private Material freezeTrailMaterial;
    
    [Header("Burn Stats")]
    [Tooltip("Amount of time the target spends burning.")]
    [HideInInspector] [SerializeField] private float burnDuration;
    [Tooltip("Defines gun and bullet colour.")]
    [HideInInspector] [SerializeField] private Material burnMaterial;
    [HideInInspector] [SerializeField] private Material burnTrailMaterial;
    
    // Set materials
    void OnEnable()
    {
        switch (behaviour)
        {
            case ElementBehaviour.None:
                material = baseMaterial;
                trailMaterial = baseTrailMaterial;
                
                break;
            case ElementBehaviour.Freeze:
                material = freezeMaterial;
                trailMaterial  = freezeTrailMaterial;

                break;
            case ElementBehaviour.Burn:
                material = burnMaterial;
                trailMaterial  = burnTrailMaterial;

                break;
        }
    }
    
    public void ApplyEffect(Collider other)
    {
        switch (behaviour)
        {
            case ElementBehaviour.Freeze:
                ApplyFreeze(other);

                break;
            case ElementBehaviour.Burn:
                ApplyBurn(other);

                break;
        }
    }
    
    

    private void ApplyFreeze(Collider other)
    {
        // Apply freeze if applicable
        if (other.TryGetComponent(out IFreezable freezable))
            freezable.Freeze(freezeAmount);
    }

    private void ApplyBurn(Collider other)
    {
        // Apply burn if applicable
        if (other.TryGetComponent(out IBurnable burnable))
        {
            burnable.Burn(burnDuration);
        }
    }


}
