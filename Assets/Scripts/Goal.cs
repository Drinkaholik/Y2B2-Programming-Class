using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    
    public UnityEvent GoalScored;
    

    void OnTriggerEnter(Collider other)
    {

        GoalScored?.Invoke();

    }
    
}
