using UnityEngine;

public class Walk : BaseMoveState
{
    private AttackHandler attackHandle;

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
        if (Input.GetKey(player.attack))
            attackHandle.Attack(AttackHandler.AttackType.STANDING);

        // walk
        if (player.isAttack)
            player.rb.velocity = new Vector2(player.moveX * player.walkSpeed, player.rb.velocity.y); 
    }

    /// Exit
    public override void Exit()
    {
        Debug.Log("Walk Exit");
    }
}
