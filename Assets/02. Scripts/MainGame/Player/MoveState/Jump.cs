using UnityEngine;

public class Jump : BaseMoveState
{
    public Jump(Player player) : base(player) { }

    /// Enter
    public override void Enter()
    {
        Debug.Log("Jump Enter");

        // animation setting
        player.ani.SetBool("isWalk", false);
        player.ani.SetBool("isRun", false);
        player.ani.SetBool("isCrouch", false);
        player.ani.SetBool("isJump", true);
        player.ani.SetBool("isClimb", false);

        // jump (1È¸¼º)
        player.isJumping = true;
        player.rb.AddForce(player.transform.up * player.jumpPower, ForceMode2D.Impulse);
    }

    /// HandleInput
    public override void ChangeStateLogic()
    {
        // state change
        if (!player.isJumping)
        {
            // walk & run
            if (player.isMoveLKey || player.isMoveRKey)
            {
                if (player.isRunKey)
                    player.ChangeState(Player.MovementState.Run);
                else
                    player.ChangeState(Player.MovementState.Walk);

                return;
            }

            // crouch
            if(player.isCrouchKey)
                player.ChangeState(Player.MovementState.Crouch);

            // idle
            player.ChangeState(Player.MovementState.Idle);
        }

        // climb
        if (player.isInLadder && player.isClimbUpKey)
        {
            player.ChangeState(Player.MovementState.Climb);
        }
    }

    /// LogicUpdate
    public override void UpdateLigic() 
    {
        // isJumping flag controll
        if (player.rb.velocity.y > 0.01f)
            player.isJumping = true;
        else
            player.isJumping = false;
    }

    /// Exit
    public override void Exit()
    {
        player.isJumping = false;
        Debug.Log("Jump Exit");
    }
}
