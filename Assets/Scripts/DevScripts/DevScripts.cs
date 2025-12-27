
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


    public class TransformUtils : MonoBehaviour
    {

        /// <summary>
        /// Rotate a gameObject towards a target position, at a given rate
        /// </summary>
        /// <param name="obj">The object to rotate</param>
        /// <param name="targetPos">The position to rotate towards</param>
        /// <param name="rate">The rate of rotation in degrees per second</param>
        public static void RotateObject(GameObject obj, Vector3 targetPos, float rate)
        {
            var targetDir = (targetPos - obj.transform.position).normalized;
        
            if (targetDir == Vector3.zero) return;
        
            var targetAngle = Quaternion.LookRotation(targetDir);
        
            obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, targetAngle, rate * Time.deltaTime);
        }

        /// <summary>
        /// Rotate a gameObject around a single axis towards a target position, at a given rate
        /// </summary>
        /// <param name="obj">The object to rotate</param>
        /// <param name="targetPos">The position to rotate towards</param>
        /// <param name="rate">The rate of rotation in degrees per second</param>
        /// <param name="axis">The axis around which to rotate</param>
        public static void RotateObject(GameObject obj, Vector3 targetPos, float rate, Vector3 axis)
        {
            var targetDir = (targetPos - obj.transform.position).normalized;
            
            var flattenedDir = Vector3.ProjectOnPlane(targetDir, axis);
        
            if (flattenedDir == Vector3.zero) return;
        
            var targetAngle = Quaternion.LookRotation(flattenedDir, axis);
        
            obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, targetAngle, rate * Time.deltaTime);
        }
        
    }
    
    
    
    
    
    
    
    
    
    
}
