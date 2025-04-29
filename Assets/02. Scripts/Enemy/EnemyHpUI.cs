using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpUI : MonoBehaviour
{
    [SerializeField] private Image enemyHp_Image;
    [SerializeField] private GameObject enemyDamage_Text;

    private int enemyMaxHp;

    public void SetEnemyHpData(int maxHp)
    {
        enemyMaxHp = maxHp;
    }

    public void UpdateEnemyHpUI(int hp)
    {
        if(!enemyHp_Image.gameObject.activeSelf) enemyHp_Image.gameObject.SetActive(true);
        enemyHp_Image.fillAmount = (float)hp / (float)enemyMaxHp;

        
    }

    public void EnemyDamageUI(int damage)
    {
        GameObject damageText = Instantiate(enemyDamage_Text, transform.position, Quaternion.identity, transform);
        damageText.GetComponent<Text>().text = damage.ToString();
        damageText.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), 3f), ForceMode2D.Impulse);
        Destroy(damageText, 0.5f);
    }
}
