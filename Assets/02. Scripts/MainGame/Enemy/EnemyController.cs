using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Player;

public class EnemyController : MonoBehaviour
{
    private enum State { IDLE, TRACE, ATTACK }

    [Header ("Stat")]
    [SerializeField] private State curState;
    [SerializeField] private float hp;
    [SerializeField] private int power;
    [SerializeField] private float speed;

    [Header("AI")]
    [SerializeField] private float yLimit;
    [SerializeField] private float traceLimit;
    [SerializeField] private float attackLimit;
    [SerializeField] private float attackCooltime;

    [Header("Asset")]
    [SerializeField] private GameObject bulelt;
    [SerializeField] private Transform bulletPosL;
    [SerializeField] private Transform bulletPosR;

    // controll data
    private bool isDie;
    private GameObject player;
    private Vector3 playerPos;
    private float dist;
    private float yDist;
    private float timer;
    private int direction;    // right : 1, left : -1

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
        timer = attackCooltime;     // 최초 1회
    }

    private void Update()
    {
        if (!isDie)
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
            sr.flipX = false;
            direction = -1;
            rb.velocity = transform.right * speed * direction;
        }
        else
        {
            sr.flipX = true;
            direction = 1;
            rb.velocity = transform.right * speed * direction;
        }
    }

    /// Attack
    private void Attack()
    {
        // transfomrm
        if (playerPos.x < this.transform.position.x)
        {
            sr.flipX = false;
            direction = -1;
            rb.velocity = Vector2.zero;
        }
        else
        {
            sr.flipX = true;
            direction = 1;
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

    /// Die
    private void Die()
    {
        // 죽는 연출 추가
        Destroy(this.gameObject);
    }

    /// Shoot
    private void Shoot()
    {
        if (direction == 1)
        {
            GameObject enemyBullet = Instantiate(bulelt, bulletPosL.position, Quaternion.identity);
            enemyBullet.GetComponent<EnemyBullet>().Init(direction, power);
        }
        else
        {
            GameObject enemyBullet = Instantiate(bulelt, bulletPosR.position, Quaternion.identity);
            enemyBullet.GetComponent<EnemyBullet>().Init(direction, power);
        }
        
    }

    // Hit
    public void Hit(int damage)
    {
        Debug.Log("아파");
        hp -= damage;

        if (hp <= 0)
        {
            hp = 0;
            isDie = true;
            Die();
        }
    }
}
