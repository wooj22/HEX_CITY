using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private Image playerHp_Image;
    [SerializeField] private Image playerCharge_Image;
    private int playerMaxHp;
    private int playerMaxCharge;

    public static PlayerUIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Player Data Setting
    public void SetPlayerUIDate(int maxHp, int maxCharge)
    {
        playerMaxHp = maxHp;
        playerMaxCharge = maxCharge;
    }

    // PlayerHpUI Update
    public void UpdatePlayerHpUI(int hp)
    {
        playerHp_Image.fillAmount = (float)hp / (float)playerMaxHp;
    }

    // PlayerChargeUI Update
    public void UpdatePlayerChargeUI(int charge)
    {
        playerCharge_Image.fillAmount = (float)charge / (float)playerMaxCharge;
    }
}
