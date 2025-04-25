using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    private int damage;
    [HideInInspector] public int direction = 1; 
    private bool isHit;
    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
        Destroy(this.gameObject, 7f);
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

    // enemy와 충돌시 소멸
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isHit = true;
            GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<EnemyController>().Hit(damage);

            ani.SetBool("isHit", true);
            AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);
            Destroy(ani.gameObject, stateInfo.length);
        }
    }
}
