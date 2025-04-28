using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpUI : MonoBehaviour
{
    [SerializeField] private Image enemyHp_Image;
    private int enemyMaxHp;

    public void SetObjectHpData(int maxHp)
    {
        enemyMaxHp = maxHp;
    }

    public void UpdatePlayerHpUI(int hp)
    {
        enemyHp_Image.fillAmount = (float)hp / (float)enemyMaxHp;
    }
}
