using UnityEngine;

public class Jump : BaseState
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

    /// Change State
    public override void ChangeStateLogic()
    {
        // state change
        if (!player.isJumping)
        {
            // walk & run
            if (player.isMoveLKey || player.isMoveRKey)
            {
                if (player.isRunKey)
                    player.ChangeState(Player.PlayerState.Run);
                else
                    player.ChangeState(Player.PlayerState.Walk);

                return;
            }

            // crouch
            if(player.isCrouchKey)
                player.ChangeState(Player.PlayerState.Crouch);

            // idle
            player.ChangeState(Player.PlayerState.Idle);
        }

        // climb
        if (player.isInLadder && player.isClimbUpKey)
        {
            player.ChangeState(Player.PlayerState.Climb);
        }
    }

    /// Logic Update
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
