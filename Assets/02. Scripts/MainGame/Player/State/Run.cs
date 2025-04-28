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

    /// Change State
    public override void ChangeStateLogic()
    {
        // walk
        if (!player.isRunKey &&
            (player.isMoveLKey || player.isMoveRKey))
        {
            player.ChangeState(Player.PlayerState.Walk);
        }

        // idle
        if (!player.isMoveLKey && !player.isMoveRKey)
            player.ChangeState(Player.PlayerState.Idle);

        // jump
        if (player.isJump2Key && player.isFloor ||
            player.isJumpKey && player.isFloor && !player.isInLadder)
        {
            player.ChangeState(Player.PlayerState.Jump);
        }

        // crouch
        if (player.isCrouchKey)
            player.ChangeState(Player.PlayerState.Crouch);

        // climb
        if (player.isInLadder && player.isClimbUpKey)
        {
            player.ChangeState(Player.PlayerState.Climb);
        }
    }

    /// Logic Update
    public override void UpdateLigic()
    {
        // input   
        player.moveX = Input.GetAxis("Horizontal");
        Debug.Log(player.moveX);

        // filp
        if (player.moveX < 0) { player.sr.flipX = true; player.lastDir = -1; }      // left
        else if (player.moveX > 0) { player.sr.flipX = false; player.lastDir = 1; } // right

        // attack flag setting
        if (player.isAttackKey)
        {
            player.isAttack = true;
            player.ani.SetBool("isAttack", true);
        }
        if (!player.isAttackKey)
        {
            player.isAttack = false;
            player.ani.SetBool("isAttack", false);
        }

        // attack
        if (player.isAttack)
            attackHandle.Attack(AttackHandler.AttackType.RUNNING);

        // run
        player.rb.velocity = new Vector2(player.lastDir * player.runSpeed, player.rb.velocity.y);
    }

    /// Exit
    public override void Exit()
    {
        Debug.Log("Run Exit");
    }
}
