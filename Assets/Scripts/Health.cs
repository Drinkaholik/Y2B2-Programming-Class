using System;
using UnityEngine;

// Describe class function here

public class Health : MonoBehaviour, IDamageable
{
    
    [SerializeField] private int maxHealth;
    private int _health;
    
    public event Action<int> OnHealthChanged;
    public event Action OnDeath;
    
    void Start()
    {
        _health = maxHealth;
        OnHealthChanged?.Invoke(_health);
       
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        OnHealthChanged?.Invoke(_health);
    }

    public void OnDestroy()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }


    // Class ends here
}
