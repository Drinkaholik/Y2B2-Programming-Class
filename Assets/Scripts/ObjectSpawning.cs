using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawning : MonoBehaviour
{

    [SerializeField] private GameObject SpawnObject;
    
    // Seconds between spawning
    [SerializeField] [Tooltip("Time in seconds for next box to spawn")] private float spawnRate;
    [SerializeField] private int maxBoxes; // Number of boxes in pool
    [SerializeField] private int despawnHeight; 
    
    // Spawn locations
    [SerializeField] private int spawnRange;
    [SerializeField] private float spawnHeight;
    
    
    private float count;
    
    [SerializeField] private List<Material> matList = new();
    
    private Queue<GameObject> boxPool = new(); // Object pool
    private List<GameObject> activeBoxes = new();
    
    
    
    void Start()
    {
        
        count = spawnRate;
        
        // Spawns all boxes and adds them to pool
        for (int i = 0; i < maxBoxes; i++)
        {
            InstantiateToPool();
        }
        
    }

    
    // Update is called once per frame6
    void Update()
    {
            
        SpawnFromPool();
            
    }

    void FixedUpdate()
    {

        HeightDespawn();


    }


    private void InstantiateToPool()
    {
        
        GameObject boxInstance = Instantiate(SpawnObject);
        
        // Change box colour //
        var number = UnityEngine.Random.Range(0, matList.Count);
        var spawnColour = matList[number];
        
        Renderer instanceRenderer = boxInstance.GetComponent<Renderer>(); // Change colour of box instance, rather than the prefab
        instanceRenderer.sharedMaterial = spawnColour; // Using sharedMaterial saves memory
        
        boxPool.Enqueue(boxInstance);
        boxInstance.gameObject.SetActive(false); // Deactivate
        
        
    }
    
    private void SpawnFromPool()
    {
       
        count -= Time.deltaTime;
        
        if ((count <= 0) && (boxPool.Count > 0))
        {
            
            // Randomize spawn position
            var xRandom = UnityEngine.Random.Range(-spawnRange, spawnRange);
            var zRandom = UnityEngine.Random.Range(-spawnRange, spawnRange);
            
            Vector3 spawnPos = new Vector3(xRandom, spawnHeight, zRandom);
            
            // Spawn box
            GameObject boxToSpawn = boxPool.Dequeue();
            boxToSpawn.SetActive(true);
            boxToSpawn.transform.position = spawnPos;
            activeBoxes.Add(boxToSpawn); // Add newly spawned box to activeBoxes list (for height checking)
            
            // Reset counter
            count = spawnRate;
        }
      

    }


    private void ReturnToPool(GameObject box)
    {
        
        box.SetActive(false);
        boxPool.Enqueue(box);
        
        
    }

    private void HeightDespawn()
    {
        
        // Loops through all boxes in the list, checking their height
        for (int i = activeBoxes.Count - 1; i >= 0; i--)
        {
            
            var currentBox = activeBoxes[i];

            if (currentBox.transform.position.y <= despawnHeight)
            {
                ReturnToPool(currentBox);
                activeBoxes.RemoveAt(i);
                
            }
            
        }
        
    }
    
    
}
