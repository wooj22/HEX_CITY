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
    public override void ChangeStateLogic()
    {
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

        // state change
        // walk
        if (!player.isRunKey &&
            (player.isMoveLKey || player.isMoveRKey))
        {
            player.ChangeState(Player.MovementState.Walk);
        }

        // idle
        if (!player.isMoveLKey && !player.isMoveRKey)
            player.ChangeState(Player.MovementState.Idle);

        // jump
        if (player.isJump2Key && player.isFloor ||
            player.isJumpKey && player.isFloor && !player.isInLadder)
        {
            player.ChangeState(Player.MovementState.Jump);
        }

        // crouch
        if (player.isCrouchKey)
            player.ChangeState(Player.MovementState.Crouch);

        // climb
        if (player.isInLadder && player.isClimbUpKey)
        {
            player.ChangeState(Player.MovementState.Climb);
        }
    }

    /// LogicUpdate
    public override void UpdateLigic()
    {
        // input   
        player.moveX = Input.GetAxis("Horizontal");
        Debug.Log(player.moveX);

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
        player.rb.velocity = new Vector2(player.lastDir * player.runSpeed, player.rb.velocity.y);
    }

    /// Exit
    public override void Exit()
    {
        Debug.Log("Run Exit");
    }
}
