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
    private float inputX;       // keycode�� �⺻ horizontal�� �ƴҰ�� ���� ����
    private float inputY;
    private Vector2 moveDir;

    // component
    private Player player;
    private Rigidbody2D rb;

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        switch (player.curState)
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
        inputX = Input.GetAxis("Horizontal");
        moveDir = transform.right * inputX;

        // walk, run, jump
        switch (player.moveState)
        {
            case Player.PlayerMoveState.WALK:
                rb.velocity = moveDir * walkSpeed;
                break;
            case Player.PlayerMoveState.RUN:
                rb.velocity = moveDir * runSpeed;
                break;
            default:
                break;
        }
    }

    private void Jump()
    {
        // TODO :: isFloor ����, move�ÿ� !isFloor�� �Ǿ 
        // ��� ������. ���� ����
        if (player.isFloor)
            rb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
    }

    private void Climb()
    {
        // �߷� ���� X
        inputY = Input.GetAxis("Vertical");
        moveDir = transform.up * inputY;
        transform.Translate(moveDir * climbSpeed * Time.deltaTime);
        rb.velocity = Vector2.zero;
    }

    private void Attack()
    {
        switch (player.attackState) 
        { 
            case Player.PlayerAttackState.ATTACK:

                break;
            case Player.PlayerAttackState.RUNATTACK:

                break;
            case Player.PlayerAttackState.SPECIALATTACK:

                break;
            default :
                break;
        }
    }
}
