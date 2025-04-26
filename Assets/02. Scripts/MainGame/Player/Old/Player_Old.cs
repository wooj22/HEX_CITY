using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Old : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] public int hp;
    [SerializeField] public int power;

    [Header ("State")]
    [SerializeField] public PlayerState curState;
    [SerializeField] public PlayerState preState;
    [SerializeField] public PlayerMoveState moveState;
    [SerializeField] public PlayerAttackState attackState;
    [SerializeField] public bool isFloor;
    [SerializeField] private bool isInLadder;
    [SerializeField] private bool isChargeMax;

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

    // data
    public enum PlayerState { IDLE, MOVE, JUMP, CLIMB, ATTACK, HIT, DIE, NONE }
    public enum PlayerMoveState { WALK, RUN, NONE }
    public enum PlayerAttackState { ATTACK, SPECIALATTACK, NONE }
    
    // controll
    private Color originalColor;
    private float gravity;

    // component
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = rb.GetComponent<SpriteRenderer>();
        originalColor = sr.color;
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
            // move state setting
            if (Input.GetKey(run))
                moveState = PlayerMoveState.RUN;
            else
                moveState = PlayerMoveState.WALK;

            if (!isFloor) return;
            else if (curState == PlayerState.ATTACK) return;
            else if (curState == PlayerState.CLIMB) return;
            else if (curState == PlayerState.HIT) return;
            else
            {
                preState = curState;
                curState = PlayerState.MOVE;
            }
        }

        // jump
        if (Input.GetKeyDown(jump2) && isFloor ||
            Input.GetKeyDown(jump) && isFloor && !isInLadder)
        {
            preState = curState;
            curState = PlayerState.JUMP;
            rb.AddForce(transform.up * 15f, ForceMode2D.Impulse);
        }

        // climb
        if ((Input.GetKey(climbDown) || Input.GetKey(climbUp)) && isInLadder)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Ladder");
            rb.gravityScale = 0;
            preState = curState;
            curState = PlayerState.CLIMB;
        }

        // climb 모드 해제
        if (curState == PlayerState.CLIMB && !isInLadder)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Player");
            rb.gravityScale = gravity;
            curState = PlayerState.IDLE;
        }

        // attack
        if (Input.GetKey(attack) && isFloor &&
            (curState == PlayerState.IDLE || curState == PlayerState.MOVE))
        {
            preState = curState;
            curState = PlayerState.ATTACK;
            attackState = PlayerAttackState.ATTACK;
        }

        // special attack
        if (Input.GetKey(specialAttack) && isChargeMax && isFloor &&
            (curState == PlayerState.IDLE || curState == PlayerState.MOVE || curState == PlayerState.JUMP))
        {
            curState = PlayerState.ATTACK;
            attackState = PlayerAttackState.SPECIALATTACK;
        }

        // idle
        // TODO :: 추상 class 기반의 FSM으로 바꾸고 idle 처리 일부 옮기기
        // Attack animation이 끝나고 -> idle
        if (!isFloor) return;
        else if (curState == PlayerState.CLIMB) return;
        else if (curState == PlayerState.HIT) return;
        else if ((Input.GetKey(moveL) || Input.GetKey(moveR) ||
            Input.GetKey(run) || Input.GetKey(jump) || Input.GetKey(jump2) ||
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
    public void Hit(int damage)
    {
        hp -= damage;

        if (hp < 0)
        {
            hp = 0;
            curState = PlayerState.DIE;
        }
        else
        {
            curState = PlayerState.HIT;
            StartCoroutine(HitColor());
            StartCoroutine(Waiting());
        }
    }

    /// Hit 연출
    IEnumerator HitColor()
    {
        int blinkCount = 3;
        float blinkInterval = 0.1f;
        Color blinkColor = new Color(0.5f, 0f, 0f, 0.7f);

        for (int i = 0; i < blinkCount; i++)
        {
            sr.color = blinkColor;
            yield return new WaitForSeconds(blinkInterval);
            sr.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.5f);
        curState = PlayerState.IDLE;
    }
}
