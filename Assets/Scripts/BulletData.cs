using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable Objects/Bullet")]
public class BulletData : ScriptableObject
{
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    
    public int Damage => damage;
    public float Speed => speed;

    
}
