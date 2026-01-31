using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Describe class function here

public class PickupSpawner : MonoBehaviour
{
    
    [SerializeField] private List<GameObject> pickups;
    [SerializeField] private List<Transform> spawnPoints;

    [SerializeField] private float spawnDelay;
    [SerializeField] private int maxSpawned;
    public static int Spawned;
    private int _nextSpawn;

    private Coroutine _routine;


    IEnumerator Spawn()
    {
        while (Spawned < maxSpawned)
        {
            if (_nextSpawn > pickups.Count - 1)
                _nextSpawn = 0;
            
            int random = UnityEngine.Random.Range(0, spawnPoints.Count);
            var spawned = Instantiate(pickups[_nextSpawn], spawnPoints[random].position, Quaternion.identity);
            
            _nextSpawn++;
            Spawned++;
            
            yield return new WaitForSeconds(spawnDelay);
            
        }
        
    }
    
    
    void Update()
    {
        if (_routine == null && Spawned < maxSpawned)
        {
            _routine = StartCoroutine(Spawn());
        }
        else if (Spawned >= maxSpawned)
        {
            StopCoroutine(_routine);
        }
        
    }
    
    
    
// Class ends here
}
