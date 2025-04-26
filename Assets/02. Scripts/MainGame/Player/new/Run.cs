using UnityEngine;

public class Run : BaseMoveState
{
    public Run(Player player) : base(player) { }

    /// Enter
    public override void Enter()
    {
        Debug.Log("Run Enter");

        // animation setting
        player.ani.SetBool("isWalk", false);
        player.ani.SetBool("isRun", true);
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
        // walk
        if (!Input.GetKey(player.run) &&
            (Input.GetKey(player.moveL) || Input.GetKey(player.moveR)))
        {
            player.ChangeState(Player.MovementState.Walk);
        }

        // idle
        if (!Input.GetKey(player.moveL) && !Input.GetKey(player.moveR))
            player.ChangeState(Player.MovementState.Idle);
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

        // run
        player.rb.velocity = new Vector2(player.moveX * player.runSpeed, player.rb.velocity.y);
    }

    /// Exit
    public override void Exit()
    {
        Debug.Log("Run Exit");
    }
}
