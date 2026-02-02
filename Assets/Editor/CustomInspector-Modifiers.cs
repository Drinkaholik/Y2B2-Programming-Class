using UnityEditor;
using UnityEngine;

// Custom editor script for bullet / gun modifiers
// Hides / displays certain properties based on their enums



[CustomEditor(typeof(ShootType))]
public class ShootTypeEditor : Editor
{
    // Enum
    private SerializedProperty _behaviourProp;
    
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
        // Link up properties //
        // Enum
        _behaviourProp = serializedObject.FindProperty("behaviour");
        
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
        switch (_behaviourProp.enumValueIndex)
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



[CustomEditor(typeof(MovementType))]
public class MovementTypeEditor : Editor
{
    // Enum
    private SerializedProperty _behaviourProp;
    
    // Sidewind props
    private SerializedProperty _amplitudeProp;
    private SerializedProperty _frequencyProp;
    private SerializedProperty _orientationProp;

    void OnEnable()
    {
        // Link properties //
        // Enum
        _behaviourProp = serializedObject.FindProperty("behaviour");
        
        // Sidewind properties
        _amplitudeProp = serializedObject.FindProperty("amplitude");
        _frequencyProp = serializedObject.FindProperty("frequency");
        _orientationProp = serializedObject.FindProperty("orientation");
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        DrawDefaultInspector();

        switch (_behaviourProp.enumValueIndex)
        {
            case (int)MovementType.MoveBehaviour.Sidewind:
                EditorGUILayout.PropertyField(_amplitudeProp);
                EditorGUILayout.PropertyField(_frequencyProp);  
                EditorGUILayout.PropertyField(_orientationProp);
                
                break;
            
            case (int)MovementType.MoveBehaviour.Spiral:
                

                break;
        }
        
        // Apply any changes made in inspector
        serializedObject.ApplyModifiedProperties();
    }
}



[CustomEditor(typeof(ElementType))]
public class ElementTypeEditor : Editor
{
    // Enum
    private SerializedProperty _behaviourProp;
    
    // Freeze props
    private SerializedProperty _freezeAmountProp;
    private SerializedProperty _freezeMaterialProp;
    
    // Burn props
    private SerializedProperty _burnDurationProp;
    private SerializedProperty _burnMaterialProp;
    
    
    void OnEnable()
    {
        // Link properties //
        // Enum
        _behaviourProp = serializedObject.FindProperty("behaviour");
        
        // Freeze properties
        _freezeAmountProp = serializedObject.FindProperty("freezeAmount");
        _freezeMaterialProp = serializedObject.FindProperty("freezeMaterial");
        
        // Burn properties
        _burnDurationProp = serializedObject.FindProperty("burnDuration");
        _burnMaterialProp = serializedObject.FindProperty("burnMaterial");
        
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        DrawDefaultInspector();

        switch (_behaviourProp.enumValueIndex)
        {
            case (int)ElementType.ElementBehaviour.Freeze:
                EditorGUILayout.PropertyField(_freezeAmountProp);
                EditorGUILayout.PropertyField(_freezeMaterialProp);  
                
                break;
            
            case (int)ElementType.ElementBehaviour.Burn:
                EditorGUILayout.PropertyField(_burnDurationProp);
                EditorGUILayout.PropertyField(_burnMaterialProp); 

                break;
        }
        
        // Apply any changes made in inspector
        serializedObject.ApplyModifiedProperties();
    }
}