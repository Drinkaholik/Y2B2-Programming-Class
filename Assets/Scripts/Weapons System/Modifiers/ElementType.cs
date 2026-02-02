using UnityEngine;

// Holds info and methods for the bullet elements
// All elements are mutually exclusive
// Base is abstract

[CreateAssetMenu(fileName = "ElementType", menuName = "Scriptable Objects/ElementType")]
public class ElementType : ScriptableObject
{

    public enum ElementBehaviour
    {
        Freeze,
        Burn
    }

    public ElementBehaviour behaviour;
    [HideInInspector] public Material elementMaterial;
    
    
    [Header("Freeze Stats")]
    [Tooltip("Amount to chill the target by. When they reach 100%, they freeze.")]
    [HideInInspector] [SerializeField] private int freezeAmount;
    [Tooltip("Defines gun and bullet colour.")]
    [HideInInspector] [SerializeField] private Material freezeMaterial;
    
    [Header("Burn Stats")]
    [Tooltip("Amount of time the target spends burning.")]
    [HideInInspector] [SerializeField] private float burnDuration;
    [Tooltip("Defines gun and bullet colour.")]
    [HideInInspector] [SerializeField] private Material burnMaterial;

    void OnEnable()
    {
        switch (behaviour)
        {
            case ElementBehaviour.Freeze:
                elementMaterial = freezeMaterial;

                break;
            case ElementBehaviour.Burn:
                elementMaterial = burnMaterial;

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
