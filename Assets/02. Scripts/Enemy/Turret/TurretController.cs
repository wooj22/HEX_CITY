using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    // stat & component
    [SerializeField] int hp;
    [SerializeField] int maxHp;
    [SerializeField] Animator effectAni1;
    [SerializeField] Animator effectAni2;

    // component
    private SpriteRenderer sr;
    private Color originalColor;
    private EnemyHpUI enemyHpUI;

    private void Start()
    {
        // getcomponent
        sr = GetComponent<SpriteRenderer>();
        enemyHpUI = GetComponentInChildren<EnemyHpUI>();

        // data setting
        originalColor = sr.color;
        hp = maxHp;

        // ui setting
        enemyHpUI.SetEnemyHpData(maxHp);
    }

    /// Hit
    public void Hit(int damage)
    {
        hp -= damage;
        enemyHpUI.UpdateEnemyHpUI(hp);
        StartCoroutine(HitColor());

        if (hp <= 0)
        {
            hp = 0;
            GetComponent<BoxCollider2D>().enabled = false;
            Die();
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
    // TODO :: Item Drop
    private void Die()
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().Enhance();
        effectAni1.SetBool("isHit", true);
        effectAni2.SetBool("isHit", true);

        AnimatorStateInfo stateInfo = effectAni1.GetCurrentAnimatorStateInfo(0);
        Destroy(this.gameObject, stateInfo.length);
    }
}
