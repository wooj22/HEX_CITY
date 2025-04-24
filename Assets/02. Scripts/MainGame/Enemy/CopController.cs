using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopController : MonoBehaviour
{
    [Header ("Stat")]
    [SerializeField] private float hp;
    [SerializeField] private float power;
    [SerializeField] private float speed;
    [SerializeField] private State curState;
    private enum State { IDLE, TRACE, ATTACK }

    [Header("AI")]
    [SerializeField] private float traceLimit;
    [SerializeField] private float attackLimit;
    
    // controll data
    private GameObject player;
    private Vector3 playerPos;
    private float dist;

    // component
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator ani;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();    
        ani = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
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

        // state
        if (dist > traceLimit) curState = State.IDLE;
        else if (dist <= attackLimit) curState = State.ATTACK;
        else curState = State.TRACE;
    }

    /// Trace
    private void Trace()
    {
        if(playerPos.x < this.transform.position.x)
        {
            sr.flipX = false;
            rb.velocity = transform.right * -speed;
        }
        else
        {
            sr.flipX = true;
            rb.velocity = transform.right * speed;
        }
    }

    /// Attack
    private void Attack()
    {
        if (playerPos.x < this.transform.position.x)
        {
            sr.flipX = false;
            rb.velocity = Vector2.zero;
        }
        else
        {
            sr.flipX = true;
            rb.velocity = Vector2.zero;
        }
    }
}
