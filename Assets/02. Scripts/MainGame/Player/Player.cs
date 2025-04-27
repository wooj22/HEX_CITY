using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("MoveState")]
    [SerializeField] MovementState moveState;
    public BaseMoveState curMoveState;
    public BaseMoveState[] moveStates;
    public enum MovementState
    {
        Idle, Walk, Run, Crouch, Jump, Climb
    }

    [Header("Player Stat")]
    public int hp;
    public int power;
    public float walkSpeed;
    public float runSpeed;
    public float climbSpeed;
    public float jumpPower;

    [Header("Player State Flags")]
    public bool isDie;
    public bool isHit;
    public bool isAttack;
    public bool isChargeMax;
    public bool isFloor;
    public bool isInLadder;
    public bool isJumping;

    [Header("Key Bindings")]
    public KeyCode moveL = KeyCode.LeftArrow;
    public KeyCode moveR = KeyCode.RightArrow;
    public KeyCode run = KeyCode.LeftShift;
    public KeyCode crouch = KeyCode.DownArrow;
    public KeyCode jump = KeyCode.UpArrow;
    public KeyCode jump2 = KeyCode.Space;
    public KeyCode climbUp = KeyCode.UpArrow;
    public KeyCode climbDown = KeyCode.DownArrow;   // ����?
    public KeyCode attack = KeyCode.D;
    public KeyCode specialAttack = KeyCode.F;

    // controll
    [HideInInspector] public float moveX;       // keycode�� �⺻ horizontal�� �ƴҰ�� ���� ����
    [HideInInspector] public float moveY;
    [HideInInspector] public int lastDir;       // right : 1, left : -1
    [HideInInspector] public float originGravity;
    private Color originColor;
   
    // Components
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator ani;

    private void Awake()
    {
        // movement states ���
        moveStates = new BaseMoveState[System.Enum.GetValues(typeof(MovementState)).Length];

        moveStates[(int)MovementState.Idle] = new Idle(this);
        moveStates[(int)MovementState.Walk] = new Walk(this);
        moveStates[(int)MovementState.Run] = new Run(this);
        moveStates[(int)MovementState.Crouch] = new Crouch(this);
        moveStates[(int)MovementState.Jump] = new Jump(this);
        moveStates[(int)MovementState.Climb] = new Climb(this);

        // get component
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // start movement state setting
        ChangeState(MovementState.Idle);

        // data setting
        originColor = sr.color;
        originGravity = rb.gravityScale;
    }

    private void Update()
    {
        if (!isDie)
        {
            curMoveState?.HandleInput();
            curMoveState?.LogicUpdate();
        }
    }

    /// Movement FSM - State Change
    public void ChangeState(MovementState state)
    {
        curMoveState?.Exit();
        curMoveState = moveStates[(int)state];
        moveState = state;
        curMoveState?.Enter();
    }

    /// Hit
    public void Hit(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            hp = 0;
            isDie = true;
            Die();
        }
        else
        {
            isHit = true;
            //ani.SetBool("isHit", true);
            StartCoroutine(HitColor());
            StartCoroutine(HitFlagRelease());
        }
    }

    /// TODO :: Die ���� ó��
    private void Die()
    {
        Debug.Log("paleyr Die!");
        this.gameObject.SetActive(false);
    }

    /// Hit ����
    IEnumerator HitColor()
    {
        int blinkCount = 3;
        float blinkInterval = 0.1f;
        Color blinkColor = new Color(0.5f, 0f, 0f, 0.7f);

        for (int i = 0; i < blinkCount; i++)
        {
            sr.color = blinkColor;
            yield return new WaitForSeconds(blinkInterval);
            sr.color = originColor;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    /// isHit false
    private IEnumerator HitFlagRelease()
    {
        AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        //ani.SetBool("isHit", false);
        isHit = false;
    }
    

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
        if (collision.gameObject.CompareTag("Floor"))
        {
            isFloor = false;
        }
    }
}
