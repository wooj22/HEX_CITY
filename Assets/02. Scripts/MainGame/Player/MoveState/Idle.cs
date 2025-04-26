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

    /// HandleInput
    public override void HandleInput()
    {
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
        // climb
        if (player.isInLadder && Input.GetKey(player.climbUp))
        {
            player.ChangeState(Player.MovementState.Climb);
        }

        // walk & run
        if (Input.GetKey(player.moveL) || Input.GetKey(player.moveR))
        {
            if (Input.GetKey(player.run))
                player.ChangeState(Player.MovementState.Run);
            else
                player.ChangeState(Player.MovementState.Walk);
        }

        // crouch
        if (Input.GetKey(player.crouch))
            player.ChangeState(Player.MovementState.Crouch);

        // jump
        if (Input.GetKeyDown(player.jump2) && player.isFloor ||
            Input.GetKeyDown(player.jump) && player.isFloor && !player.isInLadder)
        {
            player.ChangeState(Player.MovementState.Jump);
        }
    }

    /// LogicUpdate
    public override void LogicUpdate() 
    {
        // attack
        if (Input.GetKey(player.attack))
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
