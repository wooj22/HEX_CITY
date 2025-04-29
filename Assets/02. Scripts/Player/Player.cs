using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("MoveState")]
    [SerializeField] public PlayerState playerMoveState;  // enum
    public BaseMoveState curMoveState;                    // state class
    public BaseMoveState[] moveStateArr;                  // state class array
    public enum PlayerState       // state class array 접근, 관리용 enum
    {
        Idle, Walk, Run, Crouch, Jump, Climb 
    }

    [Header("Player Stat")]
    public int hp;
    [SerializeField] private int maxHp;
    public int power;
    public int initPower;
    public float walkSpeed;
    public float runSpeed;
    public float climbSpeed;
    public float jumpPower;
    private int charge;
    private int maxCharge = 10;

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
    public bool isJumpKey;      // up arrow
    public bool isJump2Key;     // space bar
    public bool isClimbUpKey;   // up arrow
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
    public KeyCode attack = KeyCode.C;
    public KeyCode specialAttack = KeyCode.F;

    // controll
    [HideInInspector] public float moveX;       // keycode가 기본 horizontal이 아닐경우 수정 요함
    [HideInInspector] public float moveY;
    [SerializeField] public int lastDir = 1;        // right : 1, left : -1 (체크용으로 인스펙터 잠깐 빼둠)
    [HideInInspector] public float originGravity;
    private Color originColor;

    // Components
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator ani;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // movement states 등록
        moveStateArr = new BaseMoveState[System.Enum.GetValues(typeof(PlayerState)).Length];

        moveStateArr[(int)PlayerState.Idle] = new Idle(this);
        moveStateArr[(int)PlayerState.Walk] = new Walk(this);
        moveStateArr[(int)PlayerState.Run] = new Run(this);
        moveStateArr[(int)PlayerState.Crouch] = new Crouch(this);
        moveStateArr[(int)PlayerState.Jump] = new Jump(this);
        moveStateArr[(int)PlayerState.Climb] = new Climb(this);

        // get component
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // game manager setting
        GameManager.Instance.player = this;

        // start movement state setting
        ChangeState(PlayerState.Idle);

        // data setting
        hp = maxHp;
        originColor = sr.color;
        originGravity = rb.gravityScale;
        
        // ui data setting
        PlayerUIManager.Instance.SetPlayerUIDate(maxHp, maxCharge);
    }

    private void Update()
    {
        if (!isDie)
        {
            KeyInputHandler();
            curMoveState?.ChangeStateLogic();
            curMoveState?.UpdateLigic();
        }
    }

    /// Movement FSM - State Change
    public void ChangeState(PlayerState state)
    {
        curMoveState?.Exit();
        curMoveState = moveStateArr[(int)state];
        this.playerMoveState = state;
        curMoveState?.Enter();
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

        // attack key input
        if (Input.GetKeyDown(attack)) isAttackKey = true;
        if (Input.GetKeyUp(attack)) isAttackKey = false;

        if(Input.GetKeyDown(specialAttack)) isSpecialAttackKey = true;
        if (Input.GetKeyUp(specialAttack)) isSpecialAttackKey = false;
    }

    /// Init
    public void PlayerInit(Vector3 position)
    {
        hp = maxHp;
        charge = maxCharge;
        power = initPower;
        transform.position = position;
        rb.velocity = Vector2.zero;

        GetComponent<AttackHandler>().AttackHandlerInit();
    }

    /// Init Power 초기화 (Boss맵으로 넘어갈 때)
    public void InitPowerInit()
    {
        initPower = power;
    }

    /// Hit
    public void Hit(int damage)
    {
        hp -= damage;
        PlayerUIManager.Instance.UpdatePlayerHpUI(hp);

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

    /// Turret 강화
    public void Enhance()
    {
        hp = maxHp;
        charge = maxCharge;
        power *= 2;

        PlayerUIManager.Instance.UpdatePlayerHpUI(hp);
        PlayerUIManager.Instance.UpdatePlayerChargeUI(charge);
        Debug.Log("Player Enhance ~~~");
    }

    /// TODO :: Die 로직 처리
    private void Die()
    {
        Debug.Log("paleyr Die!");
        GameManager.Instance.MainMapOver();
    }

    /// Goal => BossMap
    private void Goal()
    {
        GameManager.Instance.MainMapClear();
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

        // 맵 경계에 닿았을 경우 Die
        if (collision.gameObject.CompareTag("MapBoder"))
        {
            Die();
        }

        if(collision.gameObject.CompareTag("Goal"))
        {
            Goal();
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
