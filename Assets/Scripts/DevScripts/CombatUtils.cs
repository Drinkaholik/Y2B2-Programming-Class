
using System;
using UnityEngine;

namespace DevScripts
{

    public interface IDamageable
    {

        int Health {get; set;}

    }
    
    
    public class CombatUtils
    {
        private static float count;
        
        public static void TakeDamage(ref int hp, int damage)
        {
            hp -= damage;
        }

        public static void HealOnce(ref int hp, int healAmount)
        {
            
            hp += healAmount;
        }
        
        // HealOverTime has 2 overloads
        public static void HealOverTime(ref int hp, float healRate, float duration)
        {
            
            if (duration < 0)
            {
                duration -= Time.deltaTime;
                count += healRate * Time.deltaTime; // Needed for adding float values to int
                if (count >= 1)
                {
                    hp += Mathf.FloorToInt(count);
                    count = 0;
                }
                
            }

            else
            {
                count = 0;
            }
            
            

        }

        public static void HealOverTime(ref int hp, int totalHeal, float duration)
        {
            
            
            
        }
        
        
    
    
    
    }
}
