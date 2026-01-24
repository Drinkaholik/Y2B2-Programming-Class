using System.Collections;
using UnityEngine;

public class CoroutineExample : MonoBehaviour
{
    
    private Coroutine _activeRoutine;
    
    void Update()
    {
        // Start coroutine
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Null check is important so you don't create multiple instances of the same routine
            if (_activeRoutine == null)
                _activeRoutine = StartCoroutine(ExampleRoutine());
        }
        
        // Stop coroutine
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_activeRoutine != null)
                StopCoroutine(_activeRoutine);
        }
        
    }


    IEnumerator ExampleRoutine()
    {
        // Add functionality here
        yield return new WaitForSeconds(0.5f);
        
        // Reset variable so new coroutine can be activated
        _activeRoutine = null;
    }
    
    
    
// Class ends here
}
