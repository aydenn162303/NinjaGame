using UnityEngine;

public class ShootFastState : PlayerBaseState
{
    // Store reference to the state machine
    // No factory needed based on PlayerStateMachine.cs structure

    public ShootFastState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    
    private GameObject arrowPrefab; // Prefab for the arrow
    private float quickcooldown = 0.05f; // Quick cooldown for shooting
    public override void Enter()
    {
        // Logic when entering the shoot state (e.g., play animation, aim)
        Debug.Log("Player entered Shoot State");
        // Ctx.Animator.SetBool("IsShooting", true); // Example animation trigger
        arrowPrefab = stateMachine.arrowPrefab; // Accessing arrowPrefab through the stateMachine instance

        ShootArrow();
        
    }

    public override void Tick(float deltaTime)
    {
        // Logic during the shoot state (e.g., handle firing cooldown, check ammo)

        // Check for transitions out of the shoot state
        // Check for transitions out of the shoot state
        CheckSwitchStates(); // We'll keep this helper method for clarity

        quickcooldown -= deltaTime;
        if (quickcooldown <= 0)
        {
            ShootArrow();
            quickcooldown = 0.05f; // Reset cooldown
        }


    }

    public override void Exit()
    {

        // Logic when exiting the shoot state (e.g., stop animation)
        Debug.Log("Player exited Shoot State");
        // Ctx.Animator.SetBool("IsShooting", false); // Example animation reset
    }

    private void ShootArrow()
    {
        GameObject.Instantiate(arrowPrefab, stateMachine.PlayerTransform.position + new Vector3(1, 0, 0), Quaternion.identity);
    }

    // Helper method for transition checks (called from Tick)
    private void CheckSwitchStates()
    {

        if (stateMachine.InputReader.IsJumpPressed() && stateMachine.JumpsRemaining > 0)
        {
            stateMachine.SwitchState(stateMachine.JumpState);
            return; // Exit early after state switch
        }

    }

    // No InitializeSubState in the base class shown in PlayerStateMachine.cs
}