using System;
using UnityEngine;

// Describe class function here

public class Health : MonoBehaviour
{
    
    [SerializeField] private int maxHealth;
    private int health;
    
    public event Action<int> OnHealthChanged;
    public event Action OnDeath;
    
    void Start()
    {
        OnHealthChanged?.Invoke(maxHealth);
        OnHealthChanged += CheckHealth;
    }

    private void CheckHealth(int hp)
    {
        if (hp <= 0)
        {
            OnDeath?.Invoke();

        }
        
    }
    
    
    
    
// Class ends here
}
