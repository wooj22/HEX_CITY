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
    private float inputX;       // keycode가 기본 horizontal이 아닐경우 수정 요함
    private Vector2 movement;

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
                break;
            case Player.PlayerState.MOVE:
                Move();
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
        // walk, run, jump
        switch (player.moveState)
        {
            case Player.PlayerMoveState.WALK:

                break;
            case Player.PlayerMoveState.RUN:

                break;
            case Player.PlayerMoveState.JUMP:

                break;
            default:
                break;
        }
    }

    private void Climb()
    {
        // climb
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
