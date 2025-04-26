using UnityEngine;

public class Crouch : BaseMoveState
{
    public Crouch(Player player) : base(player) { }

    /// Enter
    public override void Enter()
    {
        Debug.Log("Crouch Enter");

        // animation setting
        player.ani.SetBool("isWalk", false);
        player.ani.SetBool("isRun", false);
        player.ani.SetBool("isCrouch", true);
        player.ani.SetBool("isJump", false);
        player.ani.SetBool("isClimb", false);

        // velocity zero
        player.rb.velocity = Vector2.zero;
    }

    /// HandleInput
    public override void HandleInput()
    {
        // state change
                
    }

    /// LogicUpdate
    public override void LogicUpdate() { }

    /// Exit
    public override void Exit()
    {
        Debug.Log("Crouch Exit");
    }
}
