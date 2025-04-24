using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] public int hp;
    [SerializeField] public int power;

    [Header ("State")]
    [SerializeField] public PlayerState state;
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
    [SerializeField] public KeyCode jump = KeyCode.UpArrow;
    [SerializeField] public KeyCode jump2 = KeyCode.Space;
    [SerializeField] public KeyCode climbUp = KeyCode.UpArrow;
    [SerializeField] public KeyCode climbDown = KeyCode.DownArrow;
    [SerializeField] public KeyCode attack = KeyCode.D;
    [SerializeField] public KeyCode specialAttack = KeyCode.F;

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
        if (Input.GetKey(moveL) || Input.GetKey(moveR))
        {
            if (!isFloor) return;
            if (state == PlayerState.ATTACK) return;
            if (state == PlayerState.CLIMB) return;
            if (state == PlayerState.HIT) return;

            state = PlayerState.MOVE;

            // move state setting
            if (Input.GetKey(run))
                moveState = PlayerMoveState.RUN;
            else
                moveState = PlayerMoveState.WALK;
        }

        // jump
        if (Input.GetKeyDown(jump2) && isFloor ||
            Input.GetKeyDown(jump) && isFloor && !isInLadder)
        {
            state = PlayerState.JUMP;
            rb.AddForce(transform.up * 15f, ForceMode2D.Impulse);
        }

        // climb
        if ((Input.GetKey(climbDown) || Input.GetKey(climbUp)) && isInLadder)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Ladder");
            rb.gravityScale = 0;
            state = PlayerState.CLIMB;
        }

        // climb 모드 해제
        if (state == PlayerState.CLIMB && !isInLadder)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Player");
            rb.gravityScale = gravity;
            state = PlayerState.NONE;
        }

        // attack
        if (Input.GetKeyDown(attack) &&
            (state == PlayerState.IDLE || state == PlayerState.MOVE || state == PlayerState.JUMP))
        {
            state = PlayerState.ATTACK;

            // attack state setting
            if (moveState == PlayerMoveState.RUN)
                attackState = PlayerAttackState.RUNATTACK;
            else
                attackState = PlayerAttackState.ATTACK;
        }

        // special attack
        if (Input.GetKeyDown(specialAttack) && isChargeMax && isFloor &&
            (state == PlayerState.IDLE || state == PlayerState.MOVE || state == PlayerState.JUMP))
        {
            state = PlayerState.ATTACK;
            attackState = PlayerAttackState.SPECIALATTACK;
        }

        // idle
        // TODO :: 추상 class 기반의 FSM으로 바꾸고 idle 처리 일부 옮기기
        // Attack animation이 끝나고 -> idle
        if (!isFloor) return;
        else if (state == PlayerState.CLIMB) return;
        else if ((Input.GetKey(moveL) || Input.GetKey(moveR) ||
            Input.GetKey(run) || Input.GetKey(jump) || Input.GetKey(jump2) ||
            Input.GetKey(attack) || Input.GetKey(specialAttack)))
        {
            return;
        }
        else
        {
            state = PlayerState.IDLE;
        }
    }

    /*-------------- event ---------------*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isInLadder = true;
            Debug.Log("In Ladder");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isInLadder = false;
            Debug.Log("out Ladder");
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
            state = PlayerState.DIE;
        }
        else
        {
            state = PlayerState.HIT;
        }
    }
}
