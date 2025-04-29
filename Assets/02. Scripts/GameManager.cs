using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public Player player;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        MainMapInit();
    }

    /// MainMap Init - player, monster �ʱ�ȭ
    private void MainMapInit()
    {
        Debug.Log("MainMapInit");

        player.PlayerInit(new Vector3(-7.7f, -4.07f, -1));

        SceneDirector.Instance.FadeIn();
        SoundManager.Instance.SetBGM("BGM_MainMap");
        SoundManager.Instance.FadeInBGM();
    }

    /// MainMap Over
    public void MainMapOver()
    {
        Invoke(nameof(MainMapInit), 0.5f);
    }

    /// MainMap Clear
    public void MainMapClear()
    {
        // main -> boss Player data ����
        player.InitPowerInit();

        SoundManager.Instance.FadeOutBGM();
        SceneDirector.Instance.FadeOutSceneChange("BossMap");

        // main -> boss ���� 1ȸ �ε�
        Invoke(nameof(BossMapInit), 4.7f);
        Invoke(nameof(PlayerBulletReLoading), 4.7f);
    }

    private void PlayerBulletReLoading()
    {
        player.PlayerBulletInit();
    }

    /// BossMapInit - player, boss �ʱ�ȭ
    private void BossMapInit()
    {
        Debug.Log("BossMapInit");
        player.PlayerInit(new Vector3(-8.63f, -4.07f, -1));

        SceneDirector.Instance.FadeIn();
        SoundManager.Instance.SetBGM("BGM_Boss");
        SoundManager.Instance.FadeInBGM();
    }

    /// BossMap Over
    public void BossMapOver()
    {
        Invoke(nameof(BossMapInit), 0.5f);
    }

    /// BossMap Clear
    public void BossMapClear()
    {
        SoundManager.Instance.FadeOutBGM();
        SceneDirector.Instance.FadeOutSceneChange("MainManu");
        Destroy(player.gameObject);
    }
}
