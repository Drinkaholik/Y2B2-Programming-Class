using UnityEngine;

public class StringTest : MonoBehaviour
{

    [SerializeField] private string String;
    private int Number;
    

    // Update is called once per frame
    void Update()

    {

        // Number += 1;
        //
        // if (Number <= 100)
        // {
        //     Debug.Log(Number);
        // }


        for ( Number = 1 ; Number <= 100; Number++)
        {
            Number += 1;
            Debug.Log(Number);
        }

    }
}
