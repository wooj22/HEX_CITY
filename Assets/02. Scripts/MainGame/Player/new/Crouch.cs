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
        // input   
        player.moveX = Input.GetAxis("Horizontal");
        player.moveY = Input.GetAxis("Vertical");

        // state change
        if (!Input.GetKey(player.crouch))
        {
            // idle
            if (!Input.GetKey(player.moveL) && !Input.GetKey(player.moveR))
                player.ChangeState(Player.MovementState.Idle);

            // walk & run
            if (Input.GetKey(player.moveL) || Input.GetKey(player.moveR))
            {
                if (Input.GetKey(player.run))
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

            // climb    
        }
    }

    /// LogicUpdate
    public override void LogicUpdate() 
    {
        // filp
        // left
        if (player.moveX < 0)
        {
            player.sr.flipX = true;
            player.lastDir = -1;
        }
        // right
        else if (player.moveX > 0)
        {
            player.sr.flipX = false;
            player.lastDir = 1;
        }
    }

    /// Exit
    public override void Exit()
    {
        Debug.Log("Crouch Exit");
    }
}
