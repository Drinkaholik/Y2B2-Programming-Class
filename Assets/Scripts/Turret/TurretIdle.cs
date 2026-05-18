using UnityEngine;

// Describe class function here

public class TurretIdle : TurretState
{
    
    public Color idleColour = Color.green;


    public override void OnEnterState()
    {
        if (context == null)
            context = GetComponent<TurretContext>();
        
        SetColour(idleColour);
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
        // Make it so barrel moves down to minAngle
        var nextAngle = Mathf.MoveTowardsAngle(
            context.barrel.transform.eulerAngles.x, -context.minAngle, context.barrelTurnRate * Time.deltaTime);
            
        context.barrel.transform.localRotation = Quaternion.Euler(nextAngle, 0, 0);
    }

    protected override void Transition()
    {
        // Transition to ready state
        if (!context.obstructed && context.playerDistance <= context.detectRange)
        {
            context.ChangeState(context.readyState);
        }
        
    }
    
    
    
    
    
    
// Class ends here
}
