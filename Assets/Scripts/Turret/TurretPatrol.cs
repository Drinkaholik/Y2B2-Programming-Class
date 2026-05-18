using System.Collections;
using UnityEngine;

// Describe class function here

public class TurretPatrol : TurretState
{
    
    

    private Coroutine _patrolRoutine;
    private WaitForSeconds _patrolWait;
    
    private readonly Color _patrolColour = Color.blue;
    
    
    public override void OnEnterState()
    {
        if (context == null)
            context = GetComponent<TurretContext>();
        
        SetColour(_patrolColour);
        
        _patrolWait = new WaitForSeconds(context.waitTime);
        _patrolRoutine = StartCoroutine(Patrol());
    }
    
    public override void OnExitState()
    {
        StopCoroutine(_patrolRoutine);
    }
    
    // Update logic goes here - called by context
    public override void Tick()
    {
        
        
        StateBehaviour();
        Transition();
        
    }

    protected override void StateBehaviour()
    {
        // Since it uses a coroutine, behaviour is handled in OnEnterState()
    }
    
    protected override void Transition()
    {
        // Transition to ready state
        if (!context.obstructed && context.playerDistance <= context.detectRange)
        {
            context.ChangeState(context.readyState);
        }
    }
    
    
    
    
    IEnumerator Patrol()
    {
                
        var startAngle = context.platform.transform.localEulerAngles.y;
        var leftAngle = startAngle - context.patrolAngle;
        var rightAngle = startAngle + context.patrolAngle;
                
        // Look in last seen direction for x seconds
        yield return new WaitForSeconds(context.initialWaitTime);
                
        // Rotate left, then right
        for (int i = 0; i < context.timesTurned; i++)
        {
            yield return RotateToAngle(leftAngle);
            yield return _patrolWait;
                
            yield return RotateToAngle(rightAngle);
            yield return _patrolWait;
        }
                
        // Rotate back to lastseen dir
        yield return RotateToAngle(startAngle);
        
        context.ChangeState(context.idleState);
        
    }
        
    // Handles rotation inside Patrol routine - needs to be IEnum instead of method
    IEnumerator RotateToAngle(float targetAngle)
    {
        while (Mathf.Abs(Mathf.DeltaAngle(context.platform.transform.localEulerAngles.y, targetAngle)) > 0.1f)
        {
            var currentAngle = context.platform.transform.localEulerAngles.y;
                    
            var nextAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, context.patrolTurnRate * Time.deltaTime);
                    
            context.platform.transform.localRotation = Quaternion.Euler(0, nextAngle, 0);
            yield return null;
        }
    }
    

// Class ends here
}
