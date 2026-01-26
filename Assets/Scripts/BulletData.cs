using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable Objects/Bullet")]
public class BulletData : ScriptableObject
{
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float size;
    
    public int Damage => damage;
    public float Speed => speed;
    public float Size => size;

    
}
