using UnityEngine;

public class Idle : BaseMoveState
{
    public Idle(Player player) : base(player) { }

    /// Enter
    public override void Enter()
    {
        Debug.Log("Idle Enter");

        // animation setting
        player.ani.SetBool("isWalk", false);
        player.ani.SetBool("isRun", false);
        player.ani.SetBool("isCrouch", false);
        player.ani.SetBool("isJump", false);
        player.ani.SetBool("isClimb", false);

        // velocity zero
        player.rb.velocity = Vector2.zero;
    }

    /// HandleInput
    public override void HandleInput()
    {
        // state change
        // walk & run
        if (Input.GetKey(player.moveL) || Input.GetKey(player.moveR))
        {
            if(Input.GetKey(player.run))
                player.ChangeState(Player.MovementState.Run);
            else
                player.ChangeState(Player.MovementState.Walk);
        }

        // jump
        if (Input.GetKeyDown(player.jump2) && player.isFloor ||
            Input.GetKeyDown(player.jump) && player.isFloor && !player.isInLadder)
        {
            player.ChangeState(Player.MovementState.Jump);
        }

        // crouch

        // climb

    }

    /// LogicUpdate
    public override void LogicUpdate() { }

    /// Exit
    public override void Exit()
    {
        Debug.Log("Idle Exit");
    }
}
