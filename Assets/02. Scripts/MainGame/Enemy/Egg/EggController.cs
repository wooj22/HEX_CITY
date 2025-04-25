using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private int power;
    private bool isDie;
    private Color originalColor;

    private SpriteRenderer sr;
    private Animator ani;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        originalColor = sr.color;
    }

    /// Hit
    public void Hit(int damage)
    {
        hp -= damage;
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
