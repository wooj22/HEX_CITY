using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // controll
    [SerializeField] private float speed;
    private int damage;
    private int direction;
    private bool isHit;

    // component
    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
        Destroy(this.gameObject, 1f);
    }

    private void Update()
    {
        if(!isHit) transform.Translate(transform.right * speed * direction * Time.deltaTime, Space.World);
    }

    /// 방향과 damage set
    public void Init(int dir, int power)
    {
        direction = dir;
        damage = power;
    }

    /// 플레이어와 충돌시 소멸
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isHit = true;
            GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<Player>().Hit(damage);

            ani.SetBool("isHit", true);
            AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);
            Destroy(ani.gameObject, stateInfo.length);
        }
    }
}
