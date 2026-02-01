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
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text fps;
    
    [Tooltip("Higher means smoother, but also less accurate")]
    [SerializeField] private float smoothing;
    private float smoothedFPS;
    
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
        score.text = "0";
    }

    void Update()
    {
        // Show FPS
        var actualFPS = 1/Time.unscaledDeltaTime;
        // Needs to get smaller the larger the difference between 2 values is
        // Should equal 1 if difference is enourmous, else should equal like 0.001 if they're very close
        var dynamicSmoothing = Mathf.Abs(1 - (actualFPS / smoothedFPS)) / smoothing;
        Debug.Log(dynamicSmoothing);
        smoothedFPS = Mathf.Lerp(smoothedFPS, actualFPS, dynamicSmoothing);
        fps.text = $"FPS: {Mathf.RoundToInt(smoothedFPS)}";
    }

    public void ScoreIncrease()
    {

        _score++;
        
        score.text = $"{_score}";
        
    }


    private void HealthChange(int health)
    {
        
        _healthLabel.text = $"{health}";
        
    }
    
}
