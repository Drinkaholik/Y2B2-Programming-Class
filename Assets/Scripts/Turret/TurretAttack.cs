using UnityEngine;

// Describe class function here

public class TurretAttack : TurretState
{
    
    
    public Color attackColour = Color.red;

    private float _count;
    
    public override void OnEnterState()
    {
        if (context == null)
            context = GetComponent<TurretContext>();
        
        SetColour(attackColour);
    }
    
    public override void OnExitState()
    {
        
        
    }
    
    // Update logic goes here - called by context
    public override void Tick()
    {
        StateBehaviour();
        Transition();
    }


    protected override void StateBehaviour()
    {
        PlatformRotate();
        BarrelRotate();
        
        // Shooting logic
        if (_count <= 0)
        {
            context.shootType.Shoot(context.firePoint, context.shootType, context.moveType, context.effects, context.elementType);
            _count = context.shootType.fireRateCount;
        }

        _count -= Time.deltaTime;
    }
    
    protected override void Transition()
    {
        // Transition to ready state
        if (context.playerDistance > context.attackRange || context.obstructed)
        {
            context.ChangeState(context.readyState);
        }
        
    }

  
// Class ends here
}
