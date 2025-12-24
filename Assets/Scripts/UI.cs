using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    
    [SerializeField] private TMP_Text text1;

    private int score;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text1.text = "0";
    }

    public void ScoreIncrease()
    {

        score++;
        
        text1.text = $"{score}";
        
    }
    
}
