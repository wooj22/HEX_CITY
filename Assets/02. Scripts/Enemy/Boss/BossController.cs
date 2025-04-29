using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] private int hp;
    [SerializeField] private int maxHp;
    [SerializeField] private int power;
    [SerializeField] private float speed;

    [Header("AI")]
    [SerializeField] private float traceXLimit;     // limint까지 계속 trace
    [SerializeField] private float traceYLimit;     // limint까지 계속 trace
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
    private float xDist;
    private float yDist;
    private float timer;
    private int direction;    // right : 1, left : -1
    private Color originalColor;

    // component
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private EnemyUI enemyHpUI;


    private void Start()
    {
        // getcomponent
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        enemyHpUI = GetComponentInChildren<EnemyUI>();
        player = GameObject.FindWithTag("Player");

        // data setting
        timer = attackCooltime;     // 최초 1회
        originalColor = sr.color;
        hp = maxHp;

        // ui setting
        enemyHpUI.SetEnemyHpData(maxHp);
    }

    private void Update()
    {
        if (!isDie)
        {
            PlayerDistCheak();
            Trace();
            Attack();
        }
    }

    /// State Update
    private void PlayerDistCheak()
    {
        playerPos = player.transform.position;
        xDist = Mathf.Abs(playerPos.x - this.transform.position.x);
        yDist = Mathf.Abs(playerPos.y - this.transform.position.y);
    }

    /// Trace
    private void Trace()
    {
        if (xDist > traceXLimit || yDist > traceYLimit)
        {
            if (playerPos.x < transform.position.x)
            {
                sr.flipX = false;
                direction = -1;
            }
            else
            {
                sr.flipX = true;
                direction = 1;
            }

            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    /// Attack
    private void Attack()
    {   // shoot
        timer += Time.deltaTime;

        if (xDist <= attackLimit && timer > attackCooltime)
        {
            Shoot();
            timer = 0;
        }
    }

    /// Shoot
    private void Shoot()
    {
        if (direction == 1)
        {
            GameObject enemyBullet = Instantiate(bulelt, bulletPosR.position, Quaternion.identity);
            enemyBullet.GetComponent<EnemyBullet>().Init(direction, power);
        }
        else
        {
            GameObject enemyBullet = Instantiate(bulelt, bulletPosL.position, Quaternion.identity);
            enemyBullet.GetComponent<EnemyBullet>().Init(direction, power);
        }

    }

    // Hit
    public void Hit(int damage)
    {
        hp -= damage;
        enemyHpUI.UpdateEnemyHpUI(hp);
        enemyHpUI.EnemyDamageUI(damage);
        StartCoroutine(HitColor());

        if (hp <= 0)
        {
            hp = 0;
            isDie = true;
            rb.gravityScale = 0;
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(Die());
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

    /// Die 연출
    IEnumerator Die()
    {
        Destroy(this.gameObject);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.BossMapClear();
    }
}
