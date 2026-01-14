
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
        public static void RotateAt(GameObject obj, Vector3 targetPos, float rate)
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
        public static void RotateAt(GameObject obj, Vector3 targetPos, float rate, Vector3 axis)
        {
            var targetDir = (targetPos - obj.transform.position).normalized;
            var flattenedDir = Vector3.ProjectOnPlane(targetDir, axis);
        
            if (flattenedDir == Vector3.zero) return;
        
            var targetRotation = Quaternion.LookRotation(flattenedDir, axis);
        
            obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, targetRotation, rate * Time.deltaTime);
        }
        
        
        /// <summary>
        /// Rotate a gameObject around a single axis towards a target position, at a given rate and within a limited range of angles.
        /// Requires the object to have a parent, in order to define relative direction. 
        /// </summary>
        /// <param name="obj">The object to rotate</param>
        /// <param name="targetPos">The position to rotate towards</param>
        /// <param name="rate">The rate of rotation in degrees per second</param>
        /// <param name="axis">The axis around which to rotate</param>
        /// <param name="minAngle">The minimum angle limit, based on the parent's transform</param>
        /// <param name="maxAngle">The maximum angle limit, based on the parent's transform</param>
        
        public static void RotateAt(GameObject obj, Vector3 targetPos, float rate, Vector3 axis, float minAngle, float maxAngle)
        {
            
            // 1. Find target Vector3 direction
            var targetDir = (targetPos - obj.transform.position).normalized;
            
            // 2. Flatten targetDir down to a single axis of rotation
            var flattenedDir = Vector3.ProjectOnPlane(targetDir, axis);
            
            // 3. Stop rotating if pointing in the right direction
            if (flattenedDir == Vector3.zero) return;
            
            // 4. Find angle between flattenedDir and obj.parent
            var angle = Vector3.SignedAngle(obj.transform.parent.forward, flattenedDir, axis);
            
            // 5. Only progress rotation if within angle range
            if (angle >= minAngle && angle <= maxAngle)
            {
                // 6. Convert Vector3 direction to a quaternion rotation
                var targetRotation = Quaternion.LookRotation(flattenedDir, axis);
                
                // 7. Rotate object towards target rotation
                obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, targetRotation, rate * Time.deltaTime);
                
            }
            
            
            
            
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public static void OldRotateAt(GameObject obj, Vector3 targetPos, float rate, Vector3 axis, float minAngle, float maxAngle)
        {
            var parent = obj.transform.parent;
            
            // 1. Calculate the direction and project it onto the hinge plane
            Vector3 targetDir = (targetPos - obj.transform.position).normalized;
            Vector3 flattenedDir = Vector3.ProjectOnPlane(targetDir, axis);

            if (flattenedDir != Vector3.zero)
            {
                // 2. Determine the target world rotation
                // We use 'axis' as the 'up' vector to keep the rotation math aligned with the hinge
                Quaternion targetWorldRot = Quaternion.LookRotation(flattenedDir, axis);

                // 3. Apply the rotation in world space
                obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, targetWorldRot, rate * Time.deltaTime);
            }

            // 4. Local Clamping Logic
            Vector3 localAngles = obj.transform.localEulerAngles;

            // Normalize the angle: Unity Euler angles are 0-360. 
            // This converts them to -180 to 180 so -10 is less than 45.
            float angle = localAngles.x;
            if (angle > 180) angle -= 360;
    
            // Clamp and re-apply to the local X axis (the tilt axis)
            angle = Mathf.Clamp(angle, minAngle, maxAngle);
            obj.transform.localEulerAngles = new Vector3(angle, 0, 0);
        }
        
       
        
        
        
        
    }
    
    
    
    
    
    
    
    
    
    
}
