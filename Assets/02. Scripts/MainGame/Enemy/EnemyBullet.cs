using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    private int damage;
    [HideInInspector] public int direction = 1;
    private Animator ani;
    private bool isHit;

    private void Start()
    {
        ani = GetComponent<Animator>();
        Destroy(this.gameObject, 7f);
    }

    private void Update()
    {
        if(!isHit) transform.Translate(transform.right * speed * direction * Time.deltaTime, Space.World);
    }

    /// ����� damage set
    public void Init(int dir, int power)
    {
        direction = dir;
        damage = power;
    }

    /// �÷��̾�� �浹�� �Ҹ�
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
