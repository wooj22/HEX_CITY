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
    private float moveX;       // keycode가 기본 horizontal이 아닐경우 수정 요함
    private float moveY;

    // component
    private Player player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
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
        switch (player.state)
        {
            case Player.PlayerState.IDLE:
                rb.velocity = Vector2.zero;
                break;
            case Player.PlayerState.MOVE:
                Move();
                break;
            case Player.PlayerState.JUMP:
                Jump();
                break;
            case Player.PlayerState.CLIMB:
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
                rb.velocity = new Vector2(moveX * walkSpeed, rb.velocity.y);
                break;
            case Player.PlayerMoveState.RUN:
                rb.velocity = new Vector2(moveX * runSpeed, rb.velocity.y);
                break;
            default:
                break;
        }
    }

    private void Jump()
    {
        // TODO :: isFloor 제어, move시에 !isFloor가 되어서 
        // 계속 점프됨. 수정 요함 -> 임시로 Player.cs 안에 옮겨둔 상태
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

                break;
            case Player.PlayerAttackState.RUNATTACK:
                rb.velocity = new Vector2(moveX * runSpeed, rb.velocity.y);
                break;
            case Player.PlayerAttackState.SPECIALATTACK:

                break;
            default :
                break;
        }
    }
}
