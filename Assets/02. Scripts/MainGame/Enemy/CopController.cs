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
    private float curDist;

    // component
    private Rigidbody rb;
    private Animator animator;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        StateUpdate();

        switch (curState)
        {
            case State.IDLE:
                animator.SetBool("isTrace", false);
                animator.SetBool("isAttack", false);
                break;

            case State.TRACE:
                animator.SetBool("isTrace", true);
                animator.SetBool("isAttack", false);
                Trace();
                break;

            case State.ATTACK:
                animator.SetBool("isTrace", false);
                animator.SetBool("isAttack", true);
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
        curDist = (playerPos - this.transform.position).magnitude;
        Debug.Log(curDist);

        // state
        if (curDist > traceLimit) curState = State.IDLE;
        else if (curDist <= attackLimit) curState = State.ATTACK;
        else curState = State.TRACE;
    }

    /// Trace
    private void Trace()
    {

    }

    /// Attack
    private void Attack()
    {

    }
}
