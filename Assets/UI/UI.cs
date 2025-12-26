using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    
    // Component references
    [SerializeField] private GameObject player;
    private Health playerHealth;
    
    // UI Toolkit
    [SerializeField] private UIDocument UIDoc;
    private Label _healthLabel;
    
    // UI Objects
    [SerializeField] private TMP_Text text1;
    
    private int _score;

    void Awake()
    {
        _healthLabel = UIDoc.rootVisualElement.Q<Label>("HealthLabel"); // Find healthLabel visual element
        
        playerHealth = player.GetComponent<Health>();
        playerHealth.OnHealthChanged += HealthChange; // Needs to be in awake so that its subscribed before first invocation
    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text1.text = "0";
    }

    public void ScoreIncrease()
    {

        _score++;
        
        text1.text = $"{_score}";
        
    }


    private void HealthChange(int health)
    {
        
        _healthLabel.text = $"{health}";
        
    }
    
}
