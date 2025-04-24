using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    private Animator ani;
    private bool isHit;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!isHit) transform.Translate(transform.right * -speed * Time.deltaTime, Space.World);
    }

    // 플레이어와 충돌시 소멸
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
