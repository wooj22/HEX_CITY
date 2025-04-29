using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] private int hp;
    [SerializeField] private int maxHp;
    [SerializeField] private int power;

    [Header ("AI")]
    [SerializeField] private float yLimit;
    [SerializeField] private float attackLimit;
    [SerializeField] private float attackCooltime;

    [Header("Asset")]
    [SerializeField] private GameObject bulelt;
    [SerializeField] private Transform bulletPosL;
    [SerializeField] private Transform bulletPosR;

    // controll
    private GameObject player;
    private Vector3 playerPos;
    private float   dist;
    private float   yDist;
    private int     direction;    // right : 1, left : -1
    private bool    isDie;
    private float   timer;
    private Color   originalColor;

    // compnent
    private SpriteRenderer sr;
    private Animator ani;
    private EnemyUI enemyHpUI;

    private void Start()
    {
        // getcomponent
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        enemyHpUI = GetComponentInChildren<EnemyUI>();
        player = GameObject.FindWithTag("Player");

        // data setting
        hp = maxHp;
        timer = attackCooltime;   
        originalColor = sr.color;

        // ui setting
        enemyHpUI.SetEnemyHpData(maxHp);
    }

    private void Update()
    {
        if (!isDie)
        {
            timer += Time.deltaTime;
            CheakPlayer();
        }
    }

    // Cheak Player
    private void CheakPlayer()
    {
        playerPos = player.transform.position;
        dist = (playerPos - this.transform.position).magnitude;
        yDist = Mathf.Abs(playerPos.y - this.transform.position.y);

        // attack
        if (yDist <= yLimit)
        {
            if(dist <= attackLimit)
            {
                ani.SetBool("isAttack", true);
                Attack();
            }
            else
            {
                ani.SetBool("isAttack", false);
            }
        }
        else
        {
            ani.SetBool("isAttack", false);
        }
    }

    // Attack
    private void Attack()
    {
        // direction
        if (playerPos.x < this.transform.position.x)
        {
            sr.flipX = false;
            direction = -1;
        }
        else
        {
            sr.flipX = true;
            direction = 1;
        }

        // shoot
        if (timer >= attackLimit)
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

    /// Hit
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
        yield return null;
    }
}
