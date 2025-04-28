using UnityEngine;

public class Crouch : BaseState
{
    private AttackHandler attackHandle;

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

        // attack handle get
        attackHandle = player.GetComponent<AttackHandler>();
    }

    /// Change State
    public override void ChangeStateLogic()
    {
        if (!player.isCrouchKey)
        {
            // idle
            if (!player.isMoveLKey && !player.isMoveRKey)
                player.ChangeState(Player.PlayerState.Idle);

            // walk & run
            if (player.isMoveLKey || player.isMoveRKey)
            {
                if (player.isRunKey)
                    player.ChangeState(Player.PlayerState.Run);
                else
                    player.ChangeState(Player.PlayerState.Walk);
            }

            // jump
            if (player.isJump2Key && player.isFloor ||
                player.isJumpKey && player.isFloor && !player.isInLadder)
            {
                player.ChangeState(Player.PlayerState.Jump);
            }

            // climb
            if (player.isInLadder && player.isClimbUpKey)
            {
                player.ChangeState(Player.PlayerState.Climb);
            }
        }
    }

    /// Logic Update
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
        if (Input.GetKey(player.attack))
            attackHandle.Attack(AttackHandler.AttackType.CROUCHING);
    }

    /// Exit
    public override void Exit()
    {
        Debug.Log("Crouch Exit");
    }
}
