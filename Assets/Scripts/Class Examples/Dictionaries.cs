using System.Collections.Generic;
using UnityEngine;

// Describe class function here

public class Dictionaries : MonoBehaviour
{
    
    private Dictionary<int, string> exampleDict = new();
    
    void Start()
    {
        // Add elements
        exampleDict.Add(1, "one");
        exampleDict.Add(2, "two");
        exampleDict.Add(3, "three");

        foreach (KeyValuePair<int, string> pair in exampleDict)
        {
            Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
        }
        
    }

}
