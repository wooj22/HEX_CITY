using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Image enemyHp_Image;
    [SerializeField] private GameObject enemyDamage_Text;

    private int enemyMaxHp;

    public void SetEnemyHpData(int maxHp)
    {
        enemyMaxHp = maxHp;
    }

    // Enemy Hp Bar Update
    public void UpdateEnemyHpUI(int hp)
    {
        if(!enemyHp_Image.gameObject.activeSelf) enemyHp_Image.gameObject.SetActive(true);
        enemyHp_Image.fillAmount = (float)hp / (float)enemyMaxHp;
    }

    // Damage «« ø¨√‚
    public void EnemyDamageUI(int damage)
    {
        Vector2 pos = transform.position;
        pos.y += 0.6f;
        GameObject damageText = Instantiate(enemyDamage_Text, pos, Quaternion.identity, transform);
        damageText.GetComponent<Text>().text = damage.ToString();
        damageText.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), 3f), ForceMode2D.Impulse);
        Destroy(damageText, 0.7f);
    }
}
