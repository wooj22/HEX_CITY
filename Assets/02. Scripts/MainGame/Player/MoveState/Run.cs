using UnityEngine;

public class Run : BaseMoveState
{
    private AttackHandler attackHandle;

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

        // attack handle get
        attackHandle = player.GetComponent<AttackHandler>();
    }

    /// HandleInput
    public override void HandleInput()
    {
        // input   
        player.moveX = Input.GetAxis("Horizontal");
        Debug.Log(player.moveX);

        // attack flag setting
        if (Input.GetKeyDown(player.attack))
        {
            player.isAttack = true;
            player.ani.SetBool("isAttack", true);
        }
        if (Input.GetKeyUp(player.attack))
        {
            player.isAttack = false;
            player.ani.SetBool("isAttack", false);
        }

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

        // jump
        if (Input.GetKeyDown(player.jump2) && player.isFloor ||
            Input.GetKeyDown(player.jump) && player.isFloor && !player.isInLadder)
        {
            player.ChangeState(Player.MovementState.Jump);
        }

        // crouch
        if (Input.GetKey(player.crouch))
            player.ChangeState(Player.MovementState.Crouch);

        // climb
        if (player.isInLadder && Input.GetKey(player.climbUp))
        {
            player.ChangeState(Player.MovementState.Climb);
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

        // attack
        if (player.isAttack)
            attackHandle.Attack(AttackHandler.AttackType.RUNNING);

        // run
        player.rb.velocity = new Vector2(player.moveX * player.runSpeed, player.rb.velocity.y);
    }

    /// Exit
    public override void Exit()
    {
        Debug.Log("Run Exit");
    }
}
