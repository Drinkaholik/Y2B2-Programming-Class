using System.Collections;

public interface IDamageable
{
    void TakeDamage(int damage);
    
    void OnDestroy();
}

public interface IBurnable
{
    
    void Burn(float duration);
    
}

public interface IFreezable
{
    void Freeze(int freezeAmount);
}


