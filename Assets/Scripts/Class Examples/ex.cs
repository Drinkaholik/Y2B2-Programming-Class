using UnityEngine;

public class ex : MonoBehaviour
{

    private CharacterController cc;
    private float depth;
    
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
     
        
        depth = 5;

        var volume = CalculateArea(3, 4) * depth;


    }


    private float CalculateArea(float x, float y)
    {
        
        return x * y;
        
    }

    

}
