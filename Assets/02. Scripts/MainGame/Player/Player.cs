using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] public int hp;
    [SerializeField] public int power;

    [Header ("State")]
    [SerializeField] public PlayerState curState;
    [SerializeField] private PlayerState preState;
    [SerializeField] public PlayerMoveState moveState;
    [SerializeField] public PlayerAttackState attackState;
    [SerializeField] public bool isFloor;
    [SerializeField] private bool isInLadder;
    [SerializeField] private bool isChargeMax;

    public enum PlayerState { IDLE, MOVE, JUMP, CLIMB, ATTACK, HIT, DIE, NONE }
    public enum PlayerMoveState { WALK, RUN, NONE }
    public enum PlayerAttackState { ATTACK, RUNATTACK, SPECIALATTACK, NONE }

    [Header("Key Bindings")]
    [SerializeField] public KeyCode moveL = KeyCode.LeftArrow;
    [SerializeField] public KeyCode moveR = KeyCode.RightArrow;
    [SerializeField] public KeyCode run = KeyCode.LeftShift;
    [SerializeField] public KeyCode climbUp = KeyCode.UpArrow;
    [SerializeField] public KeyCode climbDown = KeyCode.DownArrow;
    [SerializeField] public KeyCode jump = KeyCode.Space;
    [SerializeField] public KeyCode attack = KeyCode.F;
    [SerializeField] public KeyCode specialAttack = KeyCode.G;

    // component
    private Rigidbody2D rb;
    private float gravity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravity = rb.gravityScale;
    }

    private void Update()
    {
        StateUpdate();
    }

    /// State Update
    void StateUpdate()
    {
        // move
        if ((Input.GetKey(moveL) || Input.GetKey(moveR)) &&
            curState != PlayerState.JUMP &&
            curState != PlayerState.ATTACK &&
            curState != PlayerState.CLIMB &&
            curState != PlayerState.HIT)
        {
            preState = curState;
            curState = PlayerState.MOVE;

            // move state setting
            if (Input.GetKey(run))
                moveState = PlayerMoveState.RUN;
            else
                moveState = PlayerMoveState.WALK;
        }

        // jump
        if (Input.GetKeyDown(jump) && isFloor)
        {
            preState = curState;
            curState = PlayerState.JUMP;
        }

        // climb
        if ((Input.GetKey(climbDown) || Input.GetKey(climbUp)) && isInLadder)
        {
            rb.gravityScale = 0;
            preState = curState;
            curState = PlayerState.CLIMB;
        }

        // climb 모드 해제
        if (curState == PlayerState.CLIMB && !isInLadder)
        {
            rb.gravityScale = gravity;
            preState = curState;
        }

        // attack
        if (Input.GetKeyDown(attack) &&
            (curState == PlayerState.IDLE || curState == PlayerState.MOVE || curState == PlayerState.JUMP))
        {
            preState = curState;
            curState = PlayerState.ATTACK;

            // attack state setting
            if (moveState == PlayerMoveState.RUN)
                attackState = PlayerAttackState.RUNATTACK;
            else
                attackState = PlayerAttackState.ATTACK;
        }

        // special attack
        if (Input.GetKeyDown(specialAttack) && isChargeMax && isFloor &&
            (curState == PlayerState.IDLE || curState == PlayerState.MOVE || curState == PlayerState.JUMP))
        {
            preState = curState;
            curState = PlayerState.ATTACK;
            attackState = PlayerAttackState.SPECIALATTACK;
        }

        // idle
        if (!isFloor) return;
        else if (curState == PlayerState.CLIMB) return;
        else if ((Input.GetKey(moveL) || Input.GetKey(moveR) ||
            Input.GetKey(run) || Input.GetKey(jump) ||
            Input.GetKey(attack) || Input.GetKey(specialAttack)))
        {
            return;
        }
        else
        {
            preState = curState;
            curState = PlayerState.IDLE;
        }

    }

    /*-------------- event ---------------*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isInLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isInLadder = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isFloor = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isFloor = false;
        }
    }

    /// Hit
    private void Hit(int damage)
    {
        hp -= damage;

        if (hp < 0)
        {
            hp = 0;
            preState = curState;
            curState = PlayerState.DIE;
        }
        else
        {
            preState = curState;
            curState = PlayerState.HIT;
        }
    }
}
