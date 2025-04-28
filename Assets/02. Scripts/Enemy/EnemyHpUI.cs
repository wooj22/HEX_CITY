using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpUI : MonoBehaviour
{
    [SerializeField] private Image enemyHp_Image;
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
}
