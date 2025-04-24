using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopController : MonoBehaviour
{
    private enum State { IDLE, TRACE, ATTACK }

    [Header ("Stat")]
    [SerializeField] private State curState;
    [SerializeField] private float hp;
    [SerializeField] private float power;
    [SerializeField] private float speed;

    [Header("AI")]
    [SerializeField] private float yLimit;
    [SerializeField] private float traceLimit;
    [SerializeField] private float attackLimit;
    [SerializeField] private float attackCooltime;

    [Header("Asset")]
    [SerializeField] private GameObject bulelt;
    [SerializeField] private Transform bulletPos;
    
    // controll data
    private GameObject player;
    private Vector3 playerPos;
    private float dist;
    private float yDist;
    private float timer;

    // component
    private Rigidbody2D rb;
    private Animator ani;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        timer = attackCooltime;     // √÷√  1»∏
    }

    private void Update()
    {
        StateUpdate();

        switch (curState)
        {
            case State.IDLE:
                ani.SetBool("isTrace", false);
                ani.SetBool("isAttack", false);
                rb.velocity = Vector2.zero;
                break;

            case State.TRACE:
                ani.SetBool("isTrace", true);
                ani.SetBool("isAttack", false);
                Trace();
                break;

            case State.ATTACK:
                ani.SetBool("isTrace", false);
                ani.SetBool("isAttack", true);
                Attack();
                break;

            default:
                break;
            
        }
    }

    /// State Update
    private void StateUpdate()
    {
        // player cheak
        playerPos = player.transform.position;
        dist = (playerPos - this.transform.position).magnitude;
        yDist = Mathf.Abs(playerPos.y - this.transform.position.y);

        // state
        if (yDist <= yLimit) {
            if (dist > traceLimit) curState = State.IDLE;
            else if (dist <= attackLimit) curState = State.ATTACK;
            else curState = State.TRACE;
        }
        else
        {
            curState = State.IDLE;
        }

    }

    /// Trace
    private void Trace()
    {
        if(playerPos.x < this.transform.position.x)
        {
            this.transform.localRotation = new Quaternion(0, 0, 0, 0);
            rb.velocity = transform.right * -speed;
        }
        else
        {
            this.transform.localRotation = new Quaternion(0, 180, 0, 0);
            rb.velocity = transform.right * -speed;
        }
    }

    /// Attack
    private void Attack()
    {
        // transfomrm
        if (playerPos.x < this.transform.position.x)
        {
            this.transform.localRotation = new Quaternion(0, 0, 0, 0);
            rb.velocity = Vector2.zero;
        }
        else
        {
            this.transform.localRotation = new Quaternion(0, 180, 0, 0);
            rb.velocity = Vector2.zero;
        }

        // shoot
        timer += Time.deltaTime;
        if(timer > attackCooltime)
        {
            Shoot();
            timer = 0;
        }
    }

    /// Shoot
    private void Shoot()
    {
        GameObject enemyBullet = Instantiate(bulelt, bulletPos.position, this.transform.localRotation);
    }
}
