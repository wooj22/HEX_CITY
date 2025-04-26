using UnityEngine;

public class Walk : BaseMoveState
{
    public Walk(Player player) : base(player) { }

    /// Enter
    public override void Enter()
    {
        Debug.Log("Walk Enter");

        // animation setting
        player.ani.SetBool("isWalk", true);
        player.ani.SetBool("isRun", false);
        player.ani.SetBool("isCrouch", false);
        player.ani.SetBool("isJump", false);
        player.ani.SetBool("isClimb", false);
    }

    /// HandleInput
    public override void HandleInput()
    {
        // input   
        player.moveX = Input.GetAxis("Horizontal");
        player.moveY = Input.GetAxis("Vertical");

        // state change
        // idle
        if (!Input.GetKey(player.moveL) && !Input.GetKey(player.moveR))
            player.ChangeState(Player.MovementState.Idle);

        // run
        if (Input.GetKey(player.run) &&
            (Input.GetKey(player.moveL) || Input.GetKey(player.moveR)))
        {
            player.ChangeState(Player.MovementState.Run);
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

        // walk
        player.rb.velocity = new Vector2(player.moveX * player.walkSpeed, player.rb.velocity.y);
    }

    /// Exit
    public override void Exit()
    {
        Debug.Log("Walk Exit");
    }
}
