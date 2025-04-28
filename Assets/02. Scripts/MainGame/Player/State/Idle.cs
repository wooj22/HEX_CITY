using UnityEngine;

public class Idle : BaseMoveState
{
    private AttackHandler attackHandle;

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

        // attack handle get
        attackHandle = player.GetComponent<AttackHandler>();
    }

    /// Change State
    public override void ChangeStateLogic()
    {
        // climb
        if (player.isInLadder && player.isClimbUpKey)
        {
            player.ChangeState(Player.PlayerState.Climb);
        }

        // walk & run
        if (player.isMoveLKey || player.isMoveRKey)
        {
            if (player.isRunKey)
                player.ChangeState(Player.PlayerState.Run);
            else
                player.ChangeState(Player.PlayerState.Walk);
        }

        // crouch
        if (player.isCrouchKey)
            player.ChangeState(Player.PlayerState.Crouch);

        // jump
        if (player.isJump2Key && player.isFloor ||
            player.isJumpKey && player.isFloor && !player.isInLadder)
        {
            player.ChangeState(Player.PlayerState.Jump);
        }
    }

    /// Logic Update
    public override void UpdateLigic()
    {
        // input   
        player.moveX = Input.GetAxis("Horizontal");
        Debug.Log(player.moveX);

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
        {
            attackHandle.Attack(AttackHandler.AttackType.STANDING);
        } 
    }

    /// Exit
    public override void Exit()
    {
        Debug.Log("Idle Exit");
    }
}
