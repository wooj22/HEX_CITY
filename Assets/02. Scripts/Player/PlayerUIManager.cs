using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private Image playerHp_Image;
    [SerializeField] private Image playerCharge_Image;
    [SerializeField] private Image chargeBack_Image;
    private int playerMaxHp;
    private int playerMaxCharge;

    private bool isShortFalling = false;
    private Color originColor;
    private Color shortFallColor = new Color(245, 100, 103 ,255);

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

    private void Start()
    {
        originColor = chargeBack_Image.color;
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

    // Player Charge Short Fall ø¨√‚
    public void ChargeShortFall()
    {
        if (isShortFalling) return;
        StartCoroutine(ChargeShortFallCo());
    }

    IEnumerator ChargeShortFallCo()
    {
        isShortFalling = true;

        int blinkCount = 3;
        float blinkInterval = 0.1f;

        for (int i = 0; i < blinkCount; i++)
        {
            chargeBack_Image.color = shortFallColor;
            yield return new WaitForSeconds(blinkInterval);
            chargeBack_Image.color = originColor;
            yield return new WaitForSeconds(blinkInterval);
        }

        isShortFalling = false;
    }
}
