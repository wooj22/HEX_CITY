using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("MoveState")]
    [SerializeField] public PlayerState state;  // enum
    public BaseState curState;                  // state class
    public BaseState[] stateArr;                // state class array
    public enum PlayerState                     // state class array 접근, 관리용 enum
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

    [Header("Player Key Input Flags")]
    public bool isMoveLKey;
    public bool isMoveRKey;
    public bool isRunKey;
    public bool isCrouchKey;
    public bool isJumpKey;
    public bool isJump2Key;
    public bool isClimbUpKey;
    public bool isClimbDownKey;
    public bool isAttackKey;
    public bool isSpecialAttackKey;

    [Header("Key Bindings")]
    public KeyCode moveL = KeyCode.LeftArrow;
    public KeyCode moveR = KeyCode.RightArrow;
    public KeyCode run = KeyCode.LeftShift;
    public KeyCode crouch = KeyCode.DownArrow;
    public KeyCode jump = KeyCode.UpArrow;
    public KeyCode jump2 = KeyCode.Space;
    public KeyCode climbUp = KeyCode.UpArrow;
    public KeyCode climbDown = KeyCode.DownArrow;   // 삭제?
    public KeyCode attack = KeyCode.D;
    public KeyCode specialAttack = KeyCode.F;

    // controll
    [HideInInspector] public float moveX;       // keycode가 기본 horizontal이 아닐경우 수정 요함
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
        // movement states 등록
        stateArr = new BaseState[System.Enum.GetValues(typeof(PlayerState)).Length];

        stateArr[(int)PlayerState.Idle] = new Idle(this);
        stateArr[(int)PlayerState.Walk] = new Walk(this);
        stateArr[(int)PlayerState.Run] = new Run(this);
        stateArr[(int)PlayerState.Crouch] = new Crouch(this);
        stateArr[(int)PlayerState.Jump] = new Jump(this);
        stateArr[(int)PlayerState.Climb] = new Climb(this);

        // get component
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // start movement state setting
        ChangeState(PlayerState.Idle);

        // data setting
        originColor = sr.color;
        originGravity = rb.gravityScale;
    }

    private void Update()
    {
        if (!isDie)
        {
            KeyInputHandler();
            curState?.ChangeStateLogic();
            curState?.UpdateLigic();
        }
    }

    /// Movement FSM - State Change
    public void ChangeState(PlayerState state)
    {
        curState?.Exit();
        curState = stateArr[(int)state];
        this.state = state;
        curState?.Enter();
    }

    /// Player Key Input
    public void KeyInputHandler()
    {
        // move key input
        if (Input.GetKeyDown(moveL)) isMoveLKey = true;
        if (Input.GetKeyUp(moveL)) isMoveLKey = false;

        if (Input.GetKeyDown(moveR)) isMoveRKey = true;
        if (Input.GetKeyUp(moveR)) isMoveRKey = false;

        // run key input
        if (Input.GetKeyDown(run)) isRunKey = true;
        if (Input.GetKeyUp(run)) isRunKey = false;

        // crouch key input
        if (Input.GetKeyDown(crouch)) isCrouchKey = true;
        if(Input.GetKeyUp(crouch)) isCrouchKey = false;

        // jump key input
        if (Input.GetKeyDown(jump2)) isJump2Key = true;
        if (Input.GetKeyUp(jump2)) isJump2Key = false;

        // climb key input
        if (Input.GetKeyDown(climbUp))
        {
            isClimbUpKey = true;
            isJumpKey = true;  // 동일키
        }
        if (Input.GetKeyUp(climbUp))
        {
            isClimbUpKey = false;
            isJumpKey = false;  // 동일키
        }

        // climb down key input
        if (Input.GetKeyDown(climbDown)) isClimbDownKey = true;
        if (Input.GetKeyUp(climbDown)) isClimbDownKey = false;

        // attack key input
        if (Input.GetKeyDown(attack)) isAttackKey = true;
        if (Input.GetKeyUp(attack)) isAttackKey = false;

        if(Input.GetKeyDown(specialAttack)) isSpecialAttackKey = true;
        if (Input.GetKeyUp(specialAttack)) isSpecialAttackKey = false;
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

    /// TODO :: Die 로직 처리
    private void Die()
    {
        Debug.Log("paleyr Die!");
        this.gameObject.SetActive(false);
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
