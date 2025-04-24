using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float attackCooltime;

    [Header("Asset")]
    [SerializeField] private GameObject bulelt;
    [SerializeField] private Transform bulletPosL;
    [SerializeField] private Transform bulletPosR;

    // contorll
    private float moveX;       // keycode가 기본 horizontal이 아닐경우 수정 요함
    private float moveY;
    private int lastDir;       // right : 1, left : -1
    private float timer;

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
        timer = attackCooltime;
    }

    private void Update()
    {
        // input   
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        timer += Time.deltaTime;

        // filp
        // left
        if (moveX < 0)  
        {
            sr.flipX = true;
            lastDir = -1;
        }
        // right
        else if (moveX > 0)
        {
            sr.flipX = false;
            lastDir = 1;
        }

            // movement contoll
            switch (player.curState)
        {
            case Player.PlayerState.IDLE:
                rb.velocity = Vector2.zero;
                ani.SetBool("isWalk", false);
                ani.SetBool("isRun", false);
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
                ani.SetBool("isJump", false);
                ani.SetBool("isClimb", true);
                Climb();
                break;

            case Player.PlayerState.ATTACK:
                Attack();
                break;

            case Player.PlayerState.HIT:
                ani.SetTrigger("Hurt");
                break;
            case Player.PlayerState.DIE:

                break;
            default:
                break;
        }
    }

    /// Move
    private void Move()
    {
        // walk, run
        switch (player.moveState)
        {
            case Player.PlayerMoveState.WALK:
                ani.SetBool("isWalk", true);
                ani.SetBool("isRun", false);
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

    /// Jump
    private void Jump()
    {
        // TODO :: isFloor 제어, move시에 !isFloor가 되어서 
        // 계속 점프됨. 수정 요함 -> 임시로 Player.cs 안에 옮겨둔 상태
        if (player.isFloor)
        {
            //rb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    /// Climb
    private void Climb()
    {
        rb.velocity = transform.up * moveY * climbSpeed;
    }

    /// Attack
    private void Attack()
    {
        switch (player.attackState)
        {
            case Player.PlayerAttackState.ATTACK:
                if (timer >= attackCooltime)
                {
                    ani.SetTrigger("Attack");
                    Shoot();
                    timer = 0;
                }
                break;

            case Player.PlayerAttackState.SPECIALATTACK:
                if (timer >= attackCooltime)
                {
                    ani.SetTrigger("SpecialAttack");
                    Shoot();
                    timer = 0;
                }

                break;
            default:
                break;
        }
    }

    /// Shoot
    private void Shoot()
    {
        Transform bulletPos = (lastDir == -1) ? bulletPosL : bulletPosR;
        GameObject playerBullet = Instantiate(bulelt, bulletPos.position, Quaternion.identity);
        playerBullet.GetComponent<PlayerBullet>().SetDirection(lastDir);
    }
}
