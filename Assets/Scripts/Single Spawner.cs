using UnityEngine;


// Describe class function here

public class PickupSpawner : MonoBehaviour
{
    
    [SerializeField] private GameObject obj;
    private GameObject _instance;
    
    [SerializeField] private float spawnDelay;
    private float _count;
    
    private bool _spawned;
    
    
    
    void Update()
    {
        Spawn();
    }
    
    void Spawn()
    {
        if (_instance == null)
        {
            _spawned = false;
        }
        
        if (_spawned) return;
        _count += Time.deltaTime;
        if (_count >= spawnDelay)
        {
            _instance = Instantiate(obj, transform.position, transform.rotation);
            _spawned = true;
            _count = 0;
        }

        

    }
    
// Class ends here
}
