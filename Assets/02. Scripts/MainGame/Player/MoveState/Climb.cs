using UnityEngine;

public class Climb : BaseMoveState
{
    public Climb(Player player) : base(player) { }

    /// Enter
    public override void Enter()
    {
        Debug.Log("Climb Enter");

        // ladder mode setting
        player.gameObject.layer = LayerMask.NameToLayer("Ladder");
        player.rb.gravityScale = 0;

        // animation setting
        player.ani.SetBool("isWalk", false);
        player.ani.SetBool("isRun", false);
        player.ani.SetBool("isCrouch", false);
        player.ani.SetBool("isJump", false);
        player.ani.SetBool("isClimb", true);
    }

    /// Change State
    public override void ChangeStateLogic()
    {
        if (!player.isInLadder)
            player.ChangeState(Player.MovementState.Idle);
    }

    /// LogicUpdate
    public override void UpdateLigic()
    {
        // input
        player.moveY = Input.GetAxis("Vertical");

        // climb
        player.rb.velocity = player.transform.up * player.moveY * player.climbSpeed;
    }

    /// Exit
    public override void Exit()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
        player.rb.gravityScale = player.originGravity;

        Debug.Log("Climb Exit");
    }
}
