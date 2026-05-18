using UnityEngine;

// Describe class function here

public class TurretReady : TurretState
{
    
    public Color readyColour = Color.yellow;
    
    
    public override void OnEnterState()
    {
        if (context == null)
            context = GetComponent<TurretContext>();
        
        SetColour(readyColour);
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
        
    }
    
    protected override void Transition()
    {
        // Transition to patrol state
        if (context.obstructed || context.playerDistance > context.detectRange)
        {
            context.ChangeState(context.patrolState);
        }
            
        // Transition to attack state
        else if (context.playerDistance <= context.attackRange)
        {
            context.ChangeState(context.attackState);
        }
        
    }
 
    
// Class ends here
}
