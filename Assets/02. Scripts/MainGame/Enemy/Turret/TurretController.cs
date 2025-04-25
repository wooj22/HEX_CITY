using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    /// Hit
    public void Hit()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(Die());
    }

    /// Hit ±ôºý°Å¸² ¿¬Ãâ ÈÄ Die
    // TODO :: Item Drop
    IEnumerator Die()
    {
        int blinkCount = 3;
        float blinkInterval = 0.1f;

        Color originalColor = sr.color;
        Color blinkColor = new Color(0.5f, 0f, 0f, 0.7f);

        for (int i = 0; i < blinkCount; i++)
        {
            sr.color = blinkColor;
            yield return new WaitForSeconds(blinkInterval);
            sr.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }

        Destroy(this.gameObject);
    }
}
