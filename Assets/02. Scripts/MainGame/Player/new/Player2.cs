using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    [Header ("���� MOVEMENT")]
    [SerializeField] public MoveState currentState;
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

    [Header("Key Bindings")]
    public KeyCode moveL = KeyCode.LeftArrow;
    public KeyCode moveR = KeyCode.RightArrow;
    public KeyCode run = KeyCode.LeftShift;
    public KeyCode crouch = KeyCode.DownArrow;
    public KeyCode jump = KeyCode.UpArrow;
    public KeyCode jump2 = KeyCode.Space;
    public KeyCode climbUp = KeyCode.UpArrow;
    public KeyCode climbDown = KeyCode.DownArrow;
    public KeyCode attack = KeyCode.D;
    public KeyCode specialAttack = KeyCode.F;

    // controll
    [HideInInspector] public float moveX;       // keycode�� �⺻ horizontal�� �ƴҰ�� ���� ����
    [HideInInspector] public float moveY;
    [HideInInspector] public int lastDir;       // right : 1, left : -1
    [HideInInspector] public float attackTimer;
    private Color originalColor;
    private float gravity;
   
    // Components
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator ani;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ChangeState(new Idle(this));
        originalColor = sr.color;
        gravity = rb.gravityScale;
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        currentState?.HandleInput();
        currentState?.LogicUpdate();
    }

    /// Movement FSM - State Change
    public void ChangeState(MoveState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    /// Hit
    public void Hit(int damage)
    {
        hp -= damage;

        if (hp < 0)
        {
            hp = 0;
            isDie = true;
        }
        else
        {
            isHit = true;
            // hit �ִϸ��̼� ����ϰ�, ������ isHit false
            StartCoroutine(HitColor());
        }
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
            sr.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }
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
