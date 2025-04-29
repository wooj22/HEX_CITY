using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // controll
    [SerializeField] private float speed;
    private int damage;
    private int direction; 
    private bool isHit;

    // component
    private Animator ani;
    private BoxCollider2D boxCol;

    private void Start()
    {
        // get component
        ani = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Move();
    }

    // 활성화 Init
    private void OnEnable()
    {
        isHit = false;
        boxCol.enabled = true;
        ani.SetBool("isHit", false);
        StartCoroutine(ActiveFalse(1f));
    }

    /// direction & damage set => attackHandler called
    public void Init(int dir, int power)
    {
        direction = dir;
        damage = power;
    }
    
    /// Move
    public void Move()
    {
        if (!isHit) transform.Translate(transform.right * speed * direction * Time.deltaTime, Space.World);
    }

    /// enemy와 충돌시 소멸
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isHit = true;
            boxCol.enabled = false;

            if(collision.gameObject.name == "Cop" || collision.gameObject.name == "Dron")
                collision.gameObject.GetComponent<EnemyController>().Hit(damage);
            else if(collision.gameObject.name == "Turret")
                collision.gameObject.GetComponent<TurretController>().Hit(damage);
            else if(collision.gameObject.name == "Egg")
                collision.gameObject.GetComponent<EggController>().Hit(damage);

            ani.SetBool("isHit", true);
            AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);
            StartCoroutine(ActiveFalse(stateInfo.length));  
        }
    }

    // 비활성화
    private IEnumerator ActiveFalse(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
    }
}
