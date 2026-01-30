using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShootType))]
public class ShootTypeEditor : Editor
{
    // Enum
    private SerializedProperty _typeProp;
    
    // Burst props
    private SerializedProperty _burstAmountProp;
    private SerializedProperty _burstDelayProp;
    
    // Multishot props
    private SerializedProperty _bulletsProp;
    private SerializedProperty _angleProp;
    private float _dynamicMax;
    
    // Grapeshot props
    private SerializedProperty _pelletsProp;
    private SerializedProperty _spreadProp;


    void OnEnable()
    {
        // Enum
        _typeProp = serializedObject.FindProperty("shootBehaviour");
        
        // Burst props
        _burstAmountProp = serializedObject.FindProperty("burstAmount");
        _burstDelayProp = serializedObject.FindProperty("burstDelay");
        
        // Multishot props
        _bulletsProp = serializedObject.FindProperty("bullets");
        _angleProp = serializedObject.FindProperty("angle");
        
        // Grapeshot props
        _pelletsProp = serializedObject.FindProperty("pellets");
        _spreadProp = serializedObject.FindProperty("spread");
        
        
    }

    public override void OnInspectorGUI()
    {
        
        serializedObject.Update();

        ShootType instance = (ShootType)target;
        
        // Draw all properties as normal
        DrawDefaultInspector();
        
        // Display conditional properties depending on type
        switch (_typeProp.enumValueIndex)
        {
            case (int)ShootType.ShootBehaviour.Burst:
                EditorGUILayout.PropertyField(_burstAmountProp);
                EditorGUILayout.PropertyField(_burstDelayProp);
                break;

            
            
            case (int)ShootType.ShootBehaviour.Multishot:
                EditorGUILayout.PropertyField(_bulletsProp);
                
                
                instance.maxAngle = instance.totalSpread / instance.bullets;
                
                // Custom slider for value, necessary for taking dynamic values for range
                _angleProp.floatValue = Mathf.Clamp(_angleProp.floatValue, 0f, instance.maxAngle);
                _angleProp.floatValue = EditorGUILayout.Slider( 
                    "Angle",
                    _angleProp.floatValue,
                    0f,
                    instance.maxAngle
                );
                break;
            
            
            case (int)ShootType.ShootBehaviour.Grapeshot:
                EditorGUILayout.PropertyField(_pelletsProp);
                EditorGUILayout.PropertyField(_spreadProp);
                break;
        }
        // Apply any changes made in inspector
        serializedObject.ApplyModifiedProperties();




    }
    
    
    
}
