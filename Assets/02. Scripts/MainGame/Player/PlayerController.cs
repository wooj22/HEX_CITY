using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // speed data
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float jumpPower;

    // move contorll
    private float moveX;       // keycode�� �⺻ horizontal�� �ƴҰ�� ���� ����
    private float moveY;

    // component
    private Player player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator ani;

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        // input   
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        // filp
        if (moveX < 0) sr.flipX = true;
        else if (moveX > 0) sr.flipX = false;

        // movement contoll
        switch (player.curState)
        {
            case Player.PlayerState.IDLE:
                rb.velocity = Vector2.zero;
                ani.SetBool("isWalk", false);
                ani.SetBool("isRun", false);
                ani.SetBool("isRunAttack", false);
                ani.SetBool("isJump", false);
                ani.SetBool("isClimb", false);
                break;

            case Player.PlayerState.MOVE:
                Move();
                ani.SetBool("isJump", false);
                ani.SetBool("isClimb", false);
                break;

            case Player.PlayerState.JUMP:
                ani.SetBool("isJump", true);
                Jump();
                break;

            case Player.PlayerState.CLIMB:
                ani.SetBool("isWalk", false);
                ani.SetBool("isRun", false);
                ani.SetBool("isRunAttack", false);
                ani.SetBool("isJump", false);
                ani.SetBool("isClimb", true);
                Climb();
                break;

            case Player.PlayerState.ATTACK:
                Attack();
                break;

            case Player.PlayerState.HIT:

                break;
            case Player.PlayerState.DIE:

                break;
            default:
                break;
        }
    }

    private void Move()
    {
        // walk, run
        switch (player.moveState)
        {
            case Player.PlayerMoveState.WALK:
                ani.SetBool("isWalk", true);
                ani.SetBool("isRun", false);
                ani.SetBool("isRunAttack", false);
                rb.velocity = new Vector2(moveX * walkSpeed, rb.velocity.y);
                break;
            case Player.PlayerMoveState.RUN:
                ani.SetBool("isWalk", false);
                ani.SetBool("isRun", true);
                rb.velocity = new Vector2(moveX * runSpeed, rb.velocity.y);
                break;
            default:
                break;
        }
    }

    private void Jump()
    {
        // TODO :: isFloor ����, move�ÿ� !isFloor�� �Ǿ 
        // ��� ������. ���� ���� -> �ӽ÷� Player.cs �ȿ� �Űܵ� ����
        if (player.isFloor)
        {
            //rb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    private void Climb()
    {
        rb.velocity = transform.up * moveY * climbSpeed;
    }

    private void Attack()
    {
        switch (player.attackState) 
        { 
            case Player.PlayerAttackState.ATTACK:
                ani.SetTrigger("Attack");
                break;
            case Player.PlayerAttackState.RUNATTACK:
                ani.SetBool("isRunAttack", true);
                rb.velocity = new Vector2(moveX * runSpeed, rb.velocity.y);
                break;
            case Player.PlayerAttackState.SPECIALATTACK:
                ani.SetTrigger("SpecialAttack");
                break;
            default :
                break;
        }
    }
}
